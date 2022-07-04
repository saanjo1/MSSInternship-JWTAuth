using jwt_token.Models;
using System.Threading.Tasks;

namespace jwt_token
{
    public interface IUserRoleService
    {

        Task Create(UserRoles role);
        //User GetById(int id);

    }
}
