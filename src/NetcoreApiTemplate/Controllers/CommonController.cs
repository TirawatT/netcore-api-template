using Microsoft.AspNetCore.Mvc;
using NetcoreApiTemplate.Data.Repositorys;

namespace NetcoreApiTemplate.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommonController(IConfiguration _config, CommonRepository _repo) : ControllerBase
    {
        #region root path first page
        [HttpGet, Route("/")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ContentResult FirstPage()
        {
            var pathBase = Request.PathBase;
            var path = pathBase + "/swagger/";
            var html = $@"
<h1>NetcoreApiTemplate</h1>
<a href=""{path}"">go to Swagger</a>
";
            return new ContentResult
            {
                ContentType = "text/html",
                Content = html
            };
        }
        #endregion
        [HttpGet, Route("CheckDb")]
        public IActionResult CheckDb() => Ok(_repo.GetDbInfo());

        [HttpGet, Route("GetConfig")]
        public IActionResult GetConfig(string key) => Ok(_config.GetValue<string>(key));
    }
}
