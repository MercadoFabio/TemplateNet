namespace $safeprojectname$.ResponseDto.Common
{
    /// <summary>
    /// Clase genérica para representar una respuesta de operación.
    /// </summary>
    /// <typeparam name="T">Tipo de datos de la propiedad Data.</typeparam>
    public class OperationResponse<T>
    {
        public bool? Success { get; }

        public string? Message { get; }

        public T? Data { get; }

        public int? Code { get; }

        public int? TotalRows { get; }

        public string? Exception { get; }


        private OperationResponse(OperationResponseBuilder builder)
        {
            Success = builder.Success;
            Message = builder.Message;
            Data = builder.Data;
            Code = builder.Code;
            TotalRows = builder.TotalRows;
            Exception = builder.Exception;
        }

        /// <summary>
        /// Clase para construir una respuesta de operation response.
        /// </summary>
        public class OperationResponseBuilder
        {
            public bool? Success { get; private set; }

            public string? Message { get; private set; }

            public T? Data { get; private set; }

            public int? Code { get; private set; }

            public int? TotalRows { get; private set; }

            public string? Exception { get; private set; }


            public OperationResponseBuilder WithSuccess(bool success)
            {
                Success = success;
                return this;
            }

            public OperationResponseBuilder WithMessage(string message)
            {
                Message = message;
                return this;
            }

            public OperationResponseBuilder WithData(T data)
            {
                Data = data;
                return this;
            }

            public OperationResponseBuilder WithCode(int code)
            {
                Code = code;
                return this;
            }

            public OperationResponseBuilder WithTotalRows(int totalRows)
            {
                TotalRows = totalRows;
                return this;
            }

            public OperationResponseBuilder WithException(string exception)
            {
                Exception = exception;
                return this;
            }

            /// <summary>
            /// Construye una nueva instancia de OperationResponse.
            /// </summary>
            /// <returns>Devuelve una instancia de operation response.</returns>
            public OperationResponse<T> Build() => new OperationResponse<T>(this);
        }

        /// <summary>
        /// Crea un nuevo constructor de respuestas.
        /// </summary>
        /// <returns>Devuelve una nueva instancia de OperationResponseBuilder.</returns>
        public static OperationResponseBuilder CreateBuilder() => new OperationResponseBuilder();

        /// <summary>
        /// Crea una respuesta de éxito.
        /// </summary>
        /// <param name="data">Los datos a incluir en la respuesta.</param>
        /// <param name="totalRows">El número total de filas, por defecto es 0.</param>
        /// <returns>Devuelve una nueva instancia de OperationResponse con Success establecido en true.</returns>
        public static OperationResponse<T> SuccessResponse(T data, int totalRows = 0) => CreateBuilder()
            .WithSuccess(true).WithMessage(Utilities.MsjSuccess).WithData(data).WithCode(200).WithTotalRows(totalRows)
            .Build();

        /// <summary>
        /// Crea una respuesta de error para un archivo.
        /// </summary>
        /// <param name="exception">La excepción a incluir en la respuesta.</param>
        /// <returns>Devuelve una nueva instancia de OperationResponse con Success establecido en false y el mensaje de error correspondiente.</returns>
        public static OperationResponse<T> ErrorFileResponse(string exception) => CreateBuilder().WithSuccess(false)
            .WithMessage(Utilities.MsjErrorFile).WithCode(500).WithException(exception).Build();

        /// <summary>
        /// Crea una respuesta de error.
        /// </summary>
        /// <param name="exception">La excepción a incluir en la respuesta.</param>
        /// <returns>Devuelve una nueva instancia de OperationResponse con Success establecido en false y el mensaje de error correspondiente.</returns>
        public static OperationResponse<T> ErrorResponse(string exception) => CreateBuilder().WithSuccess(false)
            .WithMessage(Utilities.MsjError).WithCode(500).WithException(exception).Build();

        /// <summary>
        /// Crea una respuesta de no encontrado.
        /// </summary>
        /// <returns>Devuelve una nueva instancia de OperationResponse con Success establecido en false y el mensaje de no encontrado.</returns>
        public static OperationResponse<T> NotFoundResponse() => CreateBuilder().WithSuccess(false)
            .WithMessage(Utilities.MsjNotFound).WithCode(404).Build();

        /// <summary>
        /// Crea una respuesta de error personalizada.
        /// </summary>
        /// <param name="code">El código de estado HTTP a incluir en la respuesta.</param>
        /// <param name="message">El mensaje a incluir en la respuesta.</param>
        /// <param name="data">Los datos a incluir en la respuesta, por defecto es null.</param>
        /// <returns>Devuelve una nueva instancia de OperationResponse con Success establecido en false y el mensaje y código personalizados.</returns>
        public static OperationResponse<T> CustomErrorResponse(int code, string message, T data = default) =>
            CreateBuilder().WithSuccess(false).WithMessage(message).WithData(data).WithCode(code).Build();

        /// <summary>
        /// Convierte la respuesta a una cadena JSON.
        /// </summary>
        /// <returns>Devuelve una cadena JSON que representa la respuesta.</returns>
        public override string ToString() => JsonSerializer.Serialize(this);
    }
}
