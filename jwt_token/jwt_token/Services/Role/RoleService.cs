using jwt_token.AzureRepo;
using jwt_token.Models;
using System.Threading.Tasks;

namespace jwt_token
{
    public class RoleService : IRoleService
    {
        private IAzureRepo<Role> _rolesRepository = null;


        public RoleService(IAzureRepo<Role> roleRepository)
        {
            _rolesRepository = roleRepository;
        }


        public async Task<bool> Delete(string userId)
        {
            await _rolesRepository.Delete(userId);
            return true;
        }

        async Task IRoleService.Create(Role role)
        {
            await _rolesRepository.Create(role);
            return;
            //  throw new NotImplementedException();
        }


        public string GetUserRoles(RegisterUser user)
        {
            return "Ic OK";
        }

        public async Task<Role> Read(string userName)
        {
            var response = await _rolesRepository.Read(userName);
            return response;
        }

        public async Task<bool> Update(Role role)
        {
            await _rolesRepository.Update(role);
            return true;
        }
    }
}

