using Microsoft.AspNetCore.Mvc;
using NetcoreApiTemplate.Data.Repositorys;
using NetcoreApiTemplate.Models.Table;

namespace NetcoreApiTemplate.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeController(EmployeeRepository _repo) : ControllerBase
    {
        #region RESTful
        [HttpGet]
        public IActionResult GetAll(string department)
        {
            return Ok(_repo.GetAll(department));
        }
        [HttpGet, Route("{id}")]
        public IActionResult Get(int id) => Ok(_repo.Get(id));
        [HttpPost]
        public IActionResult Create([FromBody] Employee item) => Ok(_repo.Add(item));
        [HttpPut]
        public IActionResult Update([FromBody] Employee item) => Ok(_repo.Update(item));
        [HttpDelete, Route("{id}")]
        public IActionResult Delete(int id) => Ok(_repo.Delete(id));
        #endregion


    }
}
