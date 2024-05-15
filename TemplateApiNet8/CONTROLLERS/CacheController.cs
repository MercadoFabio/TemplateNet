namespace $safeprojectname$.Controllers
{
    /// <summary>
    /// Controlador para gestionar las operaciones relacionadas con la caché.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController(IMemoryCache cache) : BaseService(cache)
    {
        /// <summary>
        /// Elimina todas las claves de la caché.
        /// </summary>
        /// <returns>Retorna un operation response con un booleano.</returns>
        [HttpDelete("[action]")]
        [ProducesResponseType(typeof(OperationResponseSwagger<bool>), StatusCodes.Status200OK)]
        public OperationResponse<bool> ClearCache()
        {
            var data = RemoveAllKeysCache();
            return data ? Ok(data) : BadRequest<bool>(Utilities.MsjError);
        }

        /// <summary>
        /// Elimina una clave de la caché.
        /// </summary>
        /// <param name="key">clave a ser eliminada.</param>
        /// <returns>Retorna un operation response con un booleano.</returns>
        [HttpDelete("{key}")]
        [ProducesResponseType(typeof(OperationResponseSwagger<bool>), StatusCodes.Status200OK)]
        public OperationResponse<bool> ClearKeyCache(string key)
        {
            var data = RemoveKeyCache(key);
            return data ? Ok(data) : NotFound<bool>();
        }

        /// <summary>
        /// Obtiene una clave de la caché.
        /// </summary>
        /// <param name="key">clave a ser obtenida.</param>
        /// <returns>Retorna un operation response con un booleano.</returns>
        [HttpGet("{key}")]
        [ProducesResponseType(typeof(OperationResponseSwagger<bool>), StatusCodes.Status200OK)]
        public OperationResponse<bool> GetKeyCache(string key)
        {
            var data = GetCache(key);
            return data == null ? NotFound<bool>() : Ok(true);
        }

        /// <summary>
        /// Recupera todas las claves de la caché.
        /// </summary>
        /// <returns>Retorna la clase operation response con una lista de string.</returns>
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(OperationResponseSwagger<bool>), StatusCodes.Status200OK)]
        public OperationResponse<IEnumerable<string>> GetCacheKeys()
        {
            var data = GetAllKeysCache();

            return data.Any() ? Ok(GetAllKeysCache()) : NotFound<IEnumerable<string>>();
        }
    }
}