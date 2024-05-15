public class FileService : BaseService, IFileService
{
    private static readonly string ConfigAccess = AppSettings.AmazonS3.AccessKeyId;
    private static readonly string ConfigSecret = AppSettings.AmazonS3.SecretKeyId;
    private static readonly string BucketName = AppSettings.AmazonS3.Name;
    private static readonly string RegionName = AppSettings.AmazonS3.Region;

    public async Task<OperationResponse<Archivo>> Post(Archivo archivo)
    {
        try
        {
            var endpoint = RegionEndpoint.GetBySystemName(RegionName);

            var s3Client = new AmazonS3Client(
                ConfigAccess,
                ConfigSecret,
                endpoint);
            var nombreArchivo = ObtenerNombreYExtension(archivo.NombreArchivo);
            var bytes = Convert.FromBase64String(archivo.Base64);
            var archivoS3 = archivo.Ruta + nombreArchivo;
            using (s3Client)
            {
                var request = new PutObjectRequest
                {
                    BucketName = BucketName,
                    CannedACL = S3CannedACL.Private,
                    Key = archivoS3
                };
                using (var ms = new MemoryStream(bytes))
                {
                    request.InputStream = ms;
                    var response = await s3Client.PutObjectAsync(request);
                    archivo.NombreArchivo = nombreArchivo;
                    return response.HttpStatusCode == System.Net.HttpStatusCode.OK
                        ? Ok(archivo)
                        : ServerErrorFile<Archivo>(string.Empty);
                }
            }
        }
        catch (Exception ex)
        {
            return ServerErrorFile<Archivo>(ex.Message);
        }
    }

    public async Task<OperationResponse<string>> Get(string nameFile)
    {
        nameFile = WebUtility.UrlDecode(nameFile);
        var endpoint = RegionEndpoint.GetBySystemName(RegionName);

        var s3Client = new AmazonS3Client(
            ConfigAccess,
            ConfigSecret,
            endpoint);

        var request = new GetObjectRequest
        {
            BucketName = BucketName,
            Key = nameFile
        };

        var response = await s3Client.GetObjectAsync(request);
        await using var responseStream = response.ResponseStream;
        var bytes = ReadStream(responseStream);
        var base64 = Convert.ToBase64String(bytes);
        return Ok(base64);
    }

    private static byte[] ReadStream(Stream responseStream)
    {
        using var ms = new MemoryStream();
        responseStream.CopyTo(ms);
        return ms.ToArray();
    }

    private static string ObtenerNombreYExtension(string nombreArchivo)
    {
        var nombre = Path.GetFileNameWithoutExtension(nombreArchivo);
        var extension = Path.GetExtension(nombreArchivo);
        var nombreConIdentificadorUnico = $"{nombre}_{Path.GetRandomFileName().Replace(".", string.Empty)}";

        return $"{nombreConIdentificadorUnico}{extension}";
    }
}