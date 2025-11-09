using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyAzureApi.Controllers
{
    [Route("api/secret")]
    [ApiController]
    public class SecretController : ControllerBase
    {
        private readonly IConfiguration _config;
        public SecretController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("secret")]
        public IActionResult GetSecret()
        {
            var azuresqlconnection = _config["azuresqlconnection"];
            var azurestorageconnection = _config["azurestorageconnection"];
            var jstissuer = _config["jstissuer"];
            var jstkey = _config["jstkey"];

            return Ok(new
            {
                azuresqlconnection,
                azurestorageconnection,
                jstissuer,
                jstkey
            });
        }


    }
}
