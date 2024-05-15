namespace $safeprojectname$.ResponsesSwagger
{
    /// <summary>
    /// Respuesta de operación Swagger.
    /// </summary>
    /// <typeparam name="T">Tipo de datos de la respuesta.</typeparam>
    public class OperationResponseSwagger<T>
    {
        /// <summary>
        /// Indica si la operación fue exitosa.
        /// </summary>
        public bool? Success { get; }

        /// <summary>
        /// Mensaje de la operación.
        /// </summary>
        public string? Message { get; }

        /// <summary>
        /// Datos de la respuesta.
        /// </summary>
        public T? Data { get; }

        /// <summary>
        /// Código de la operación.
        /// </summary>
        public int? Code { get; }

        /// <summary>
        /// Número total de filas.
        /// </summary>
        public int? TotalRows { get; }

        /// <summary>
        /// Excepción de la operación.
        /// </summary>
        public string? Exception { get; }
    }
}
