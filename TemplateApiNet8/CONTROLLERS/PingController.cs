namespace $safeprojectname$.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PingController : BaseController
    {
        /// <summary>
        /// Verifica el estado de salud de la app.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(OperationResponseSwagger<string>), StatusCodes.Status200OK)]
        public IActionResult Get()
         => Return(OperationResponse<string>.SuccessResponse("Pong"));
    }
}
