using Microsoft.AspNetCore.Mvc;
using NetcoreApiTemplate.Models.Dtos.Authorize;
using NetcoreApiTemplate.Services.Authen;

namespace NetcoreApiTemplate.Controllers
{
    [Route("api/[controller]")]
    public class AuthorizeController(AuthorizeService _service) : ControllerBase
    {
        [HttpPost, Route("Login")]
        public IActionResult Login([FromBody] LoginRequestDto item) => Ok(_service.Login(item));

    }
}
