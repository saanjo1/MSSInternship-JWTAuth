using Azure;
using Azure.Data.Tables;
using jwt_token.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace jwt_token.AzureRepo
{
    public class RolesRepository : IAzureRepo<Role>
    {
        public string ConnnectionString { get; set; }
        public string TableName { get; set; }
        public TableServiceClient _azureTableClient { get; set; }

        public RolesRepository(string connnectionString, string tableName)
        {
            ConnnectionString = connnectionString??throw new ArgumentNullException("ConnnectionString", "ConnnectionString IS NULL");
            TableName = tableName?? throw new ArgumentNullException("TableName", "TableName IS NULL");
            _azureTableClient = new TableServiceClient(ConnnectionString);
            _azureTableClient.CreateTableIfNotExists(TableName);
            
        }

        public async Task Create(Role record)
        {
            var createdUser = new RoleTableEntity()
            {
                ETag = new Azure.ETag("*"),
                PartitionKey = record.Id.ToString(),
                RowKey = record.Id.ToString(), 
                Name= record.Name,

                Timestamp = DateTimeOffset.UtcNow
            };
            Response response = null;
            try
            {
                response = await _azureTableClient.GetTableClient(TableName).AddEntityAsync(createdUser);
            }
            catch (Exception ex)
            {
                response = await _azureTableClient.GetTableClient(TableName).UpdateEntityAsync(createdUser, createdUser.ETag);
            }
            Console.WriteLine(response);
        }
        //public User Authenticate(string username, string password)
        //{
        //    User user = this.ReadUser(username); //ovo valjda treba da provjeri usera po name-u
        //                                         //sad ovo naredno treba provjeriti po passwordu 

        //    if (user.Password == password)
        //    {
        //        //  var user = _users.SingleOrDefault(x => x.Username == username && x.Password == password); //edit

        //        // return null if user not found
        //        if (user == null)
        //            return null;

        //        // authentication successful so generate jwt token
        //        var tokenHandler = new JwtSecurityTokenHandler();
        //        var secret = System.Environment.GetEnvironmentVariable("SECRET");
        //        var key = Encoding.ASCII.GetBytes(secret);
        //        var tokenDescriptor = new SecurityTokenDescriptor
        //        {
        //            Subject = new ClaimsIdentity(new Claim[]
        //            {
        //            new Claim(ClaimTypes.Name, user.Id.ToString()),
        //            new Claim(ClaimTypes.Role, user.Role)
        //            }),
        //            Expires = DateTime.UtcNow.AddDays(7),
        //            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //        };
        //        var token = tokenHandler.CreateToken(tokenDescriptor);
        //        user.Token = tokenHandler.WriteToken(token);

        //        return user.WithoutPassword();
        //    }
        //}
        public async Task<bool> Delete(string rola)
        { 
           
            Response response = null;
            try
            {
                response = await _azureTableClient.GetTableClient(TableName).DeleteEntityAsync(rola,rola);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
              //  response = await _azureTableClient.GetTableClient(TableName).UpdateEntityAsync(createdUser, createdUser.ETag);
            }
            Console.WriteLine(response);
            return true;

        }

        public async Task<bool> Update(Role role)
        {
            RoleTableEntity createdRole = new RoleTableEntity();
            createdRole.RowKey = role.Id.ToString();
            createdRole.PartitionKey = role.Id.ToString();
            createdRole.Name = role.Name;
            createdRole.ETag = new Azure.ETag("*");

            Response response = null;
            try
            {
                response = await _azureTableClient.GetTableClient(TableName).UpdateEntityAsync(createdRole, createdRole.ETag);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
                //  response = await _azureTableClient.GetTableClient(TableName).UpdateEntityAsync(createdUser, createdUser.ETag);
            }
            Console.WriteLine(response);
            return true;

        }

        public Task<bool> Update(string roleid)
        {
            throw new NotImplementedException();
        }

        public Task<Role> Read(string userName)
        {
            RoleTableEntity response = null;
            try
            {
                response = _azureTableClient.GetTableClient(TableName).GetEntity<RoleTableEntity>(userName, userName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Role role = new Role();
            role.Name = response.Name;
            role.ETag = response.ETag;
            role.Id = Guid.Parse(response.PartitionKey);
            return Task.FromResult(role);
        }

        public Task<List<Role>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Check(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}

