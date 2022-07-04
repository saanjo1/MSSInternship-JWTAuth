using jwt_token.AzureRepo;
using jwt_token.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jwt_token.Controllers
{
    public class RoleController : Controller
    {
        IRoleService _roleService = null;
        public RoleController(IRoleService roleService)
        {

            _roleService = roleService;

        }

        //  [AllowAnonymous]
        [HttpPost("createrole")]
            public async Task<IActionResult> CreateRoles()
            {  
            //var user = await _userService.Authenticate(model.Username, model.Password);

            //if (user == null)
            //    return BadRequest(new { message = "Username or password is incorrect" });

            //await _azureRepo.CreateUser(user);
                 await _roleService.Create(new Role() { Id = Guid.NewGuid(), Name = "Rola" });
                return Ok("Ic OK");
            }
        [HttpDelete("deleterole")]
        public async Task<IActionResult> DeleteRole(string roleid)
        {
            _roleService.Delete(roleid);
            return Ok("Ic OK");
        }
        [HttpGet("getrole")]
        [Authorize]
        public Task<Role> Readrole(string id)
        {
           var response = _roleService.Read(id);
            return response;
        }

        //[HttpGet("getrole")]
        //public async Task<IActionResult> UpdateRole(string Roleiid)
        //{
        //    //_roleService.Update()
        //    return Ok("Ic OK");
        //}

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
