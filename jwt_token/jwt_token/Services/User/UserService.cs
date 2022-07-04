using Azure.Data.Tables;
using jwt_token.Helpers;
using jwt_token.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace jwt_token
{
    public class UserService : IUserService
        {
            public string ConnnectionString { get; set; }
            public string TableName { get; set; }
            public TableServiceClient _azureTableClient { get; set; }

            IAzureRepo<Models.RegisterUser> _registeredUsers;

            public UserService(string connnectionString, string tableName)
            {
                ConnnectionString = connnectionString ?? throw new ArgumentNullException("ConnnectionString", "ConnnectionString IS NULL");
                TableName = tableName ?? throw new ArgumentNullException("TableName", "TableName IS NULL");
                _azureTableClient = new TableServiceClient(ConnnectionString);
                _azureTableClient.CreateTableIfNotExists(TableName);

            }



            private readonly AppSettings _appSettings;

            public UserService(IOptions<AppSettings> appSettings, IAzureRepo<Models.RegisterUser> registereduser)
            {
                _appSettings = appSettings.Value;
                _registeredUsers = registereduser;
            }

            public async Task<RegisterUser> Authenticate(string username, string password)
            {
                Task<bool> user1 = _registeredUsers.Check(username, password);

                if (user1.Result == false)
                    return null;

                var user = await _registeredUsers.Read(username);
                // return null if user not found

                // authentication successful so generate jwt token
                var tokenHandler = new JwtSecurityTokenHandler();
                var secret = System.Environment.GetEnvironmentVariable("SECRET");
                var key = Encoding.ASCII.GetBytes(secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                user.Token = tokenHandler.WriteToken(token);
                RegisterUser _user = new RegisterUser()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role,
                    Email = user.Email,
                    Token = user.Token,
                    Username = user.Username,
                    Id = user.Id
                };

                return _user;
            }

            public List<RegisterUser> GetAll()
            {
                var temp = _registeredUsers.GetAll();
                return temp.Result; // ovo modifikuj da vraca iz baze 
            }

            public async Task<RegisterUser> GetById(string id)
            {
                var user = await _registeredUsers.Read(id);
                return user;

            }

            public async Task<bool> Delete(string userid)
            {
                var temp = await _registeredUsers.Delete(userid);
                return true;

            }


            public async Task<bool> Put(RegisterUser userid)
            {
                var temp = await _registeredUsers.Update(userid);
                return true;
            }

        public async Task<RegisterUser> GetUserProfile(string id)
        {
            var temp = await _registeredUsers.GetAll();
            foreach (var item in temp)
            {
                if(item.Id == id)
                    return item;
            }
            //return new LoginModel { Username = "Jay", Password = "123456" };
            return null;
        }

        public bool isValidUserInformation(LoginModel model)
        {
            Task<bool> isValid = _registeredUsers.Check(model.Username, model.Password);

            if (isValid.Result == false)
                return false;
            return true;
        }

        public LoginModel GetUserDetails()
        {
            throw new NotImplementedException();
        }
    }

    }

