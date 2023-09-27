using final_project_be.DataAccess;
using final_project_be.Models;
using final_project_be.DTOs.User;
using final_project_be.DTOs.UserLevel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using final_project_be.Emails;
using final_project_be.Email.Template;

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

        // get all user
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userDataAccess.GetAll();
            return Ok(users);
        }

        // send mail user
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


        // activated user after register
        [HttpGet("ActivateUser")]
        public IActionResult ActivateUser(Guid userId, string email)
        {
            try
            {
                User? user = _userDataAccess.CheckUser(email);

                if (user == null)
                    return BadRequest("Activation Failed");

                if (user.Is_active == 1)
                    return BadRequest("Account has been activated");

                bool result = _userDataAccess.ActivatedUser(userId);

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



        // insert user
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO userDTO)
        {
            try
            {

                Guid defaultUserLevelId = new Guid("8a1b3833-a5c6-4a5e-891a-8d75a4ce279e"); // set default value user level to "user"

                User user = new User
                {
                    Id = Guid.NewGuid(),
                    Name = userDTO.Name,
                    Email = userDTO.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password),
                    Id_user_level = defaultUserLevelId,
                    Is_active = 0,

                };

                bool result = _userDataAccess.CreateUserAccount(user);

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

        // send email for activation user
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
                {"userId", user.Id.ToString() },
                {"email", user.Email }
            };

            string callback = QueryHelpers.AddQueryString("https://localhost:7091/api/User/ActivateUser", param);

            //string body = "Please confirm account by clicking this <a href=\"" + callback + "\"> Link</a>";

            string body = _emailService.GetMailTemplate("EmailActivation", new ActivationModel()
            {
                Email = user.Email,
                Link = callback
            });


            EmailModel emailModel = new EmailModel(to, subject, body);
            bool mailResult = await _emailService.SendAsync(emailModel, new CancellationToken());

            return mailResult;
        }


        // forget password
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                    return BadRequest("Email is empty");

                bool sendMail = await SendEmailForgetPassword(email);

                if (sendMail)
                {
                    return Ok("Mail sent");
                }
                else
                {
                    return StatusCode(500, "Error");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


        // send email forget password
        private async Task<bool> SendEmailForgetPassword(string email)
        {
            // send email
            List<string> to = new List<string>();
            to.Add(email);

            string subject = "Forget Password";

            var param = new Dictionary<string, string?>
                    {
                        {"email", email }
                    };

            string callbackUrl = QueryHelpers.AddQueryString("http://localhost:3000/createpassword", param);

            string body = "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>";

            EmailModel mailModel = new EmailModel(to, subject, body);

            bool mailResult = await _emailService.SendAsync(mailModel, new CancellationToken());

            return mailResult;

        }

        // reset password
        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword([FromBody] ResetPasswordDTO resetPassword)
        {
            try
            {
                if (resetPassword == null)
                    return BadRequest("No Data");

                if (resetPassword.Password != resetPassword.ConfirmPassword)
                {
                    return BadRequest("Password doesn't match");
                }

                bool reset = _userDataAccess.ResetPassword(resetPassword.Email, BCrypt.Net.BCrypt.HashPassword(resetPassword.Password));

                if (reset)
                {
                    return Ok("Reset password OK");
                }
                else
                {
                    return StatusCode(500, "Error");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
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

            if (user.Is_active == 0)
            {
                return Unauthorized("Your Account has not ACTIVATED");
            }

            bool isVerified = BCrypt.Net.BCrypt.Verify(credential.Password, user.Password);

            UserLevel? userLevel = _userDataAccess.GetUserLevel(user.Id_user_level);


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
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, userLevel.Name)
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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