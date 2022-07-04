using jwt_token.Models;
using System;
using System.Threading.Tasks;

namespace jwt_token
{
    public class UserRoleService : IUserRoleService
    {
        private IAzureRepo<UserRoles> _rolesRepository = null;


        public UserRoleService(IAzureRepo<UserRoles> roleRepository)
        {
            _rolesRepository = roleRepository;
        }

     
        public async Task<bool> Delete(string userId)
        {
            _rolesRepository.Delete(userId);
            return true;
        }

        public Task<RegisterUser> Read()
        {
            //_rolesRepository.Read();
            throw new NotImplementedException();


        }

        public Task<bool> Update(UserRoles record)
        {
           _rolesRepository.Update(record);
            throw new NotImplementedException();

        }


        Task IUserRoleService.Create(UserRoles role)
        {
            _rolesRepository.Create(role);
            return Task.CompletedTask;
        }

      
        public string GetUserRoles(RegisterUser user)
        {

            //var user = await _userService.Authenticate(model.Username, model.Password);

            //if (user == null)
            //    return BadRequest(new { message = "Username or password is incorrect" });

            //await _azureRepo.CreateUser(user);
            //await _roleService.Create(new Role() { Id = new Guid(), Name = "Rola" });
            return "Ic OK";
        }
        //[Allow
    }
}

