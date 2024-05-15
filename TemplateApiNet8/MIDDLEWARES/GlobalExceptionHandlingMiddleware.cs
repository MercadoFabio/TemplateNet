namespace $safeprojectname$.Middlewares
{
    /// <summary>
    /// Middleware para el manejo global de excepciones.
    /// </summary>
    /// <remarks>
    /// Constructor de la clase GlobalExceptionHandlingMiddleware.
    /// </remarks>
    /// <param name="next">Delegate de la solicitud siguiente.</param>
    /// <param name="logger">Logger para el middleware.</param>
    public class GlobalExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger = logger;

        /// <summary>
        /// Método para invocar el middleware.
        /// </summary>
        /// <param name="context">Contexto de la solicitud HTTP.</param>
        /// <returns><see cref="Task"/> Representa el asincronismo del metodo.</returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Método para manejar las excepciones.
        /// </summary>
        /// <param name="context">Contexto de la solicitud HTTP.</param>
        /// <param name="exception">Excepción ocurrida.</param>
        /// <returns>Tarea asincrónica.</returns>
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return context.Response.WriteAsync(OperationResponse<object>.ErrorResponse(exception.Message).ToString());
        }
    }
}