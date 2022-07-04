using jwt_token.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace jwt_token
{
    public interface IUserService
    {


        Task<RegisterUser> Authenticate(string username, string password);
        List<RegisterUser> GetAll();
        Task<RegisterUser> GetById(string id);

        Task<bool> Delete(string userid);
        Task<bool> Put(RegisterUser userid);
        bool isValidUserInformation(LoginModel model);
        LoginModel GetUserDetails();
        Task<RegisterUser> GetUserProfile(string id);
    }
}

