using jwt_token.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace jwt_token.Controllers
{

    public class UserRoleController : Controller
    {
        IUserRoleService _roleService = null;
        public UserRoleController(IUserRoleService roleService)
        {

            _roleService = roleService;

        }

        //  [AllowAnonymous]
        [HttpPost("createuserrole")]
            public async Task<IActionResult> CreateUserRole([FromBody] UserRoles userrole)
            {  
            //var user = await _userService.Authenticate(model.Username, model.Password);

            //if (user == null)
            //    return BadRequest(new { message = "Username or password is incorrect" });

            //await _azureRepo.CreateUser(user);
                 await _roleService.Create(new UserRoles() {UserId = userrole.UserId, RoleId = userrole.RoleId });
                return Ok("Ic OK");
            }
            //[AllowAnonymous]
            //[HttpPost("authenticate2")]
            //public async Task<IActionResult> Authenticate2([FromBody] AuthenticateModel model)
            //{
            //    var user = await _userService.Authenticate(model.Username, model.Password); //ne znam jel ovjde okej, jel treba vuci iz registrovanih

            //    if (user == null)
            //        return BadRequest(new { message = "Username or password is incorrect" });

            //    await _azureRepo.CreateUser(user);
            //    return Ok(user);
            //}

            //[AllowAnonymous]
            //[HttpPost("registar")]
            //public async Task<IActionResult> RegistarUser([FromBody] RegisterUser model)
            //{

            //    await _azureRepo.CreateUser(model);

            //    //if (user == null)
            //    //    return BadRequest(new { message = "Username or password is incorrect" });

            //    //await _azureRepo.CreateUser(user);
            //    var repo = await _azureRepo.ReadUser(model.Username);
            //    return Ok("ok.");
            //}

            //[Authorize(Roles = Role.Admin)]
            //[HttpGet]
            //public IActionResult GetAll()
            //{
            //    var users = _userService.GetAll();
            //    return Ok(users);
            //}

            //[HttpGet("{id}")]
            //public IActionResult GetById(int id)
            //{
            //    var currentUserId = int.Parse(User.Identity.Name);
            //    if (id != currentUserId && !User.IsInRole(Role.Admin))
            //        return Forbid();

            //    var user = _userService.GetById(id);

            //    if (user == null)
            //        return NotFound();

            //    return Ok(user);
            //}
        //[HttpGet]
        //[Route"userroles"]
        //public Task<List<string,string>> GetUserRoles(string userid)
        //{

        //}
    }
    
}
