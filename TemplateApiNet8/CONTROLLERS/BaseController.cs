namespace $safeprojectname$.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// Maneja el retorno de respuestas de operación comunes, como NotFound, BadRequest, y Ok.
        /// </summary>
        /// <typeparam name="T">Tipo de datos de la respuesta.</typeparam>
        /// <param name="response">Respuesta de operación a manejar.</param>
        /// <returns>Una acción de resultado que representa la respuesta.</returns>
        protected IActionResult Return<T>(OperationResponse<T> response)
        {
            return response.Code switch
            {
                404 => NotFound(response),
                400 => BadRequest(response),
                _ => Ok(response),
            };
        }

    }
}
