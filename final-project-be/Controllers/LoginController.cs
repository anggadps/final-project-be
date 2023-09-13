using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace final_project_be.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [Authorize(Roles = "admin")]
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok("GetAll method");
        }
    }
}
