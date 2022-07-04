using jwt_token.Models;
using System.Threading.Tasks;

namespace jwt_token
{
    public interface IRoleService
    {

        Task Create(Role role);
        Task<bool> Delete(string roleid);
        Task<bool> Update(Role roleid);
        // Task Read();
        Task<Role> Read(string userName);

        //User GetById(int id);

    }
}
