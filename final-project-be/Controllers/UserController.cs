using final_project_be.DataAccess;
using final_project_be.Models;
using final_project_be.DTOs.User;
using final_project_be.DTOs.UserLevel;
using final_project_be.Emails;
using final_project_be.Emails.Template;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.WebUtilities;

namespace final_project_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class UserController : ControllerBase
    {
        private readonly UserDataAccess _userDataAccess;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;


        public UserController(UserDataAccess userDataAccess, IConfiguration configuration, EmailService emailService)
        {
            _userDataAccess = userDataAccess;
            _configuration = configuration;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userDataAccess.GetAll();
            return Ok(users);
        }

        // insert user
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO userDTO)
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
                    id_user_level = userLevel.Id,
                    IsActivated = false
                };

                bool result = _userDataAccess.CreateUserAccount(user, userLevel);

                if (result)
                {
                    bool mailResult = await SendMailActivation(user);
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

        [HttpGet("ActivateUser")]
        public IActionResult ActivateUser(Guid userId, string Email)
        {
            try
            {
                User? user = _userDataAccess.CheckUser(Email);

                if (user == null)
                    return BadRequest("Activation Failed");

                if (user.IsActivated == true)
                    return BadRequest("Account has been activated");

                bool result = _userDataAccess.AcitvateUser(userId);

                if (result)

                    return Ok("User Activated Successfully");
                else
                    return BadRequest("Activation Failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Send mail
        [HttpPost("SendMailUser")]
        public async Task<IActionResult> SendMailUser([FromBody] string mailTo)
        {
            List<string> to = new List<string>();
            to.Add(mailTo);

            string subject = "Test Email FS10";
            string body = "Hallo, First Email";

            EmailModel model = new EmailModel(to, subject, body);

            bool sendMail = await _emailService.SendAsync(model, new CancellationToken());

            if (sendMail)
                return Ok("Send");
            else
                return StatusCode(500, "Error");
        }

        private async Task<bool> SendMailActivation(User user)
        {
            if (user == null)
                return false;

            if (string.IsNullOrEmpty(user.Email))
                return false;

            List<string> to = new List<string>();
            to.Add(user.Email);

            string subject = "Account Activation";

            var param = new Dictionary<string, string>()
            {
                {"Id", user.Id.ToString() },
                { "Email", user.Email }
            };

            string callback = QueryHelpers.AddQueryString("https://localhost:7091/api/User/ActivateUser", param);


            string body = _emailService.GetMailTemplate("EmailActivation", new ActivationModel()
            {
                Email = user.Email,
                Link = callback
            });

            EmailModel emailModel = new EmailModel(to, subject, body);
            bool mailResult = await _emailService.SendAsync(emailModel, new CancellationToken());

            return mailResult;
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