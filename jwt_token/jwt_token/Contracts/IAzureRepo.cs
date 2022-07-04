using System.Collections.Generic;
using System.Threading.Tasks;


namespace jwt_token
{
    public interface IAzureRepo <T>
    {  
        Task Create(T record);//Vratiti creirani objekat ili true or false
        Task <T> Read(string userName);
        Task <bool> Update(T record);
        Task <bool>  Delete(string userId);
        Task<List<T>> GetAll();
        Task<bool> Check(string username, string password); 




    }
}
