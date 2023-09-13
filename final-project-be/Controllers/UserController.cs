using final_project_be.DataAccess;
using final_project_be.Models;
using final_project_be.DTOs.User;
using final_project_be.DTOs.UserLevel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace final_project_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class UserController : ControllerBase
    {
        private readonly UserDataAccess _userDataAccess;
        private readonly IConfiguration _configuration;
        public UserController(UserDataAccess userDataAccess, IConfiguration configuration)
        {
            _userDataAccess = userDataAccess;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userDataAccess.GetAll();
            return Ok(users);
        }

        // insert user
        [HttpPost("CreateUser")]
        public IActionResult CreateUser([FromBody] UserDTO userDTO)
        {
            try
            {
                UserLevel userLevel = new UserLevel
                {
                    Id = Guid.NewGuid(),
                    Name = userDTO.UserLevel
                };

                User user = new User
                {
                    Id = Guid.NewGuid(),
                    Name = userDTO.Name,
                    Email = userDTO.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password),
                    id_user_level = userLevel.Id
                };

                bool result = _userDataAccess.CreateUserAccount(user, userLevel);

                if (result)
                {
                    return StatusCode(201, userDTO);
                }
                else
                {
                    return StatusCode(500, "Data Not Created");
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }

        // Login
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRequestDTO credential)
        {
            if (credential == null)
                return BadRequest("Data should be inputed");

            if (string.IsNullOrEmpty(credential.Email) || string.IsNullOrEmpty(credential.Password))
                return BadRequest("Email or Password should be inputed");

            User? user = _userDataAccess.CheckUser(credential.Email);

            if (user == null)
                return BadRequest("Email or Password is incorrectttt");



            bool isVerified = BCrypt.Net.BCrypt.Verify(credential.Password, user.Password);

            if (!isVerified)
            {
                return BadRequest("Email or Password is incorrectyaa");
            }
            else
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("JwtConfig:Key").Value));

                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                };

                var signingCredential = new SigningCredentials(
                    key, SecurityAlgorithms.HmacSha256Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(10),
                    SigningCredentials = signingCredential
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                var securityToken = tokenHandler.CreateToken(tokenDescriptor);

                string token = tokenHandler.WriteToken(securityToken);

                return Ok(new LoginResponseDTO
                {
                    Token = token,
                });
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

    internal class LoginResponseDTO
    {
        public string Token { get; set; } = string.Empty;
    }
}