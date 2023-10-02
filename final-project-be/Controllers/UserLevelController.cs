using final_project_be.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace final_project_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLevelController : ControllerBase
    {
        private readonly UserLevelDataAccess _userLevelDataAccess;
        public UserLevelController(UserLevelDataAccess userLevelDataAccess)
        {
            _userLevelDataAccess = userLevelDataAccess;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var user_levels = _userLevelDataAccess.GetAll();
            return Ok(user_levels);
        }
    }
}
