using jwt_token.AzureRepo;
using jwt_token.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace jwt_token.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private IUserService _userService;
        private IAzureRepo<RegisterUser> _registerUserRepo;

        public UserController(IUserService userService, IAzureRepo<RegisterUser> registerUserRepo)
        {
            _userService = userService ?? throw new ArgumentNullException("_userService", "_userService is null");
            _registerUserRepo = registerUserRepo ?? throw new ArgumentNullException("_registerUserRepo", "_registerUserRepo is null");
        }
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginModel model)
        {
            var checkUser = _userService.isValidUserInformation(model); //ne znam jel ovjde okej, jel treba vuci iz registrovanih
            if (checkUser)
            {
                var user = await _registerUserRepo.Read(model.Username);
                var tokenString = GenerateJwtToken(user);
                user.Token = tokenString;

                return Ok(user);

            }
            else
            {
                return BadRequest("Username or password is not valid.");
            }
        }

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet(nameof(GetResult))]
        public IActionResult GetResult()
        {
            return Ok("API Validated");
        }

        private string GenerateJwtToken(RegisterUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = System.Environment.GetEnvironmentVariable("SECRET");
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
         {
             new Claim("id", user.Id),
             new Claim("Firstname", user.FirstName),
             new Claim("LastName", user.LastName),
             new Claim("Email", user.Email),
             new Claim("Username", user.Username),
             new Claim("Role", user.Role),
         }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegistarUser([FromBody] RegisterUser model)
        {
            await _registerUserRepo.Create(model);

            var RegisteredUser = await _registerUserRepo.Read(model.Username);
            if (RegisteredUser.Id != null)
                return Ok("User registered.");
            return Ok("An error occured while registering a new user.");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<RegisterUser> GetUserById(string id)
        {
            try
            {
                var user = await _userService.GetById(id);
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

        }

        [Authorize]
        [HttpGet("profil")]
        public async Task<RegisterUser> Profil()
        {
            try
            {
                var _user = (RegisterUser)HttpContext.Items["User"];
                var user = await _userService.GetById(_user.Id);
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

        }

        [AllowAnonymous]
        [HttpDelete]
        public bool DeleteUser(string userid)
        {
            var users = _userService.Delete(userid);
            return true;
        }
        [AllowAnonymous]
        [HttpPut]
        public async Task<IActionResult> PutUserAsync(RegisterUser user)
        {
            var users = await _userService.Put(user);
            return Ok();
        }


    }
}
