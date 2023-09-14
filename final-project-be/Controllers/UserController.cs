using final_project_be.DataAccess;
using final_project_be.Models;
using final_project_be.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace final_project_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly UserDataAccess _userDataAccess;
        public UserController(UserDataAccess userDataAccess)
        {
            _userDataAccess = userDataAccess;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userDataAccess.GetAll();
            return Ok(users);
        }

        // insert user
        [HttpPost]
        public IActionResult Post([FromBody] UserDTO userDto)
        {
            if (userDto == null)
                return BadRequest("Data should be inputed");

            User user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Password = userDto.Password,
                id_user_level = userDto.id_user_level
            };

            bool result = _userDataAccess.Insert(user);

            if (result)
            {
                return StatusCode(201, user.Id);
            }
            else
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // update user
        [HttpPut]
        public IActionResult Put(Guid id, [FromBody] UserDTO userDto)
        {
            if (userDto == null)
                return BadRequest("Data should be inputed");

            User user = new User
            {
                Id = Guid.NewGuid(),
                Name = userDto.Name,
                Email = userDto.Email,
                Password = userDto.Password,
                id_user_level = userDto.id_user_level
            };

            bool result = _userDataAccess.Update(id, user);

            if (result)
            {
                return NoContent();
            }
            else
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // delete user
        [HttpDelete]
        public IActionResult Delete(Guid id)
        {
            bool result = _userDataAccess.Delete(id);

            if (result)
            {
                return NoContent();
            }
            else
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}