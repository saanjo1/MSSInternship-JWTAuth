using Azure;
using Azure.Data.Tables;
using jwt_token.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace jwt_token.AzureRepo
{
    public class UserRepository : IAzureRepo<Models.RegisterUser>
    {
        public string ConnnectionString { get; set; }
        public string TableName { get; set; }
        public TableServiceClient _azureTableClient { get; set; }

        public UserRepository(string connnectionString, string tableName)
        {
            ConnnectionString = connnectionString ?? throw new ArgumentNullException("ConnnectionString", "ConnnectionString IS NULL");
            TableName = tableName ?? throw new ArgumentNullException("TableName", "TableName IS NULL");
            _azureTableClient = new TableServiceClient(ConnnectionString);
            _azureTableClient.CreateTableIfNotExists(TableName);

        }

        public async Task Create(Models.RegisterUser record)
        {
            var guid = Guid.NewGuid();
            var registerUser = new RegisterUserEntity()
            {
                ETag = new Azure.ETag("*"),
                PartitionKey = guid.ToString(),
                RowKey = guid.ToString(),
                Timestamp = DateTimeOffset.UtcNow,
                FirstName = record.FirstName,
                LastName = record.LastName,
                Password = record.Password,
                Email = record.Email,
                Role = record.Role,
                Id = guid.ToString(),
                Username = record.Username
            };
            Response response = null;
            try
            {
                response = await _azureTableClient.GetTableClient(TableName).AddEntityAsync(registerUser);
            }
            catch (Exception ex)
            {
                response = await _azureTableClient.GetTableClient(TableName).UpdateEntityAsync(registerUser, registerUser.ETag);

            }
            Console.WriteLine(response);
        }

        //public async Task Create(RegisterUser record)
        //{
        //    var createdUser = new RegisterUserEntity()
        //    {
        //        ETag = new Azure.ETag("*"),
        //        PartitionKey = record.Username,
        //        RowKey = record.Username,
        //        Id = Guid.NewGuid().ToString(),
        //        FirstName = record.FirstName,

        //        LastName = record.LastName,
        //        Password = record.Password,
        //        Username = record.Username,

        //        Timestamp = DateTimeOffset.UtcNow
        //    };
        //    Response response = null;
        //    try
        //    {
        //        response = await _azureTableClient.GetTableClient(TableName).AddEntityAsync(createdUser);
        //    }
        //    catch (Exception ex)
        //    {
        //        response = await _azureTableClient.GetTableClient(TableName).UpdateEntityAsync(createdUser, createdUser.ETag);
        //    }
        //    Console.WriteLine(response);
        //}






        //public async Task<RegisterUser> Read(string userName)
        //{
        //    Response<RegisterUserEntity> response = null;
        //    try
        //    {
        //        response = await _azureTableClient.GetTableClient(TableName).GetEntityAsync<RegisterUserEntity>(userName, userName);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write(ex.ToString());
        //        return null;
        //    }
        //    var userfromEntity = new RegisterUser() {

        //        Id =response.Value.Id,
        //        FirstName = response.Value.FirstName,
        //        LastName = response.Value.LastName,
        //        Password = response.Value.Password,
        //        Username = response.Value.Username,
        //        RegisterUser = response.Value.RegisterUser
        //    };
        //    return userfromEntity;
        //}





       async Task<Models.RegisterUser> IAzureRepo<Models.RegisterUser>.Read(string userName)
        {
            RegisterUserEntity deviceTableEntityList = new RegisterUserEntity();
            try
            {
                var filter = $"PartitionKey eq '{userName}' or Username eq '{userName}'";
                var query =  _azureTableClient.GetTableClient(TableName).Query<RegisterUserEntity>(filter: filter);


                foreach (RegisterUserEntity item in query)
                {
                    deviceTableEntityList = item;
                }


            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                return null;
            }
            var userfromEntity = new RegisterUser()
            {

                Id = deviceTableEntityList.RowKey,
                FirstName = deviceTableEntityList.FirstName,
                LastName = deviceTableEntityList.LastName,
                Password = deviceTableEntityList.Password,
                Username = deviceTableEntityList.Username,
                Email = deviceTableEntityList.Email,
                Token = deviceTableEntityList.Token,
                Role = deviceTableEntityList.Role
                //Token = deviceTableEntityList.
            };
            return userfromEntity;
        }

        public async Task<List<RegisterUser>> GetAll()
        {
            Response<RegisterUserEntity> response = null;
            try
            {
                //TableContinuationToken token = null;
                //var entities = new List<RegisterUserEntity>();
                //do
                //{
                //    var query = new TableQuery<RegisterUserEntity>();
                //    var queryResult = _azureTableClient.GetTableClient(TableName).que.ExecuteQuerySegmented(new TableQuery<RegisterUserEntity>(), token);
                //    entities.AddRange(queryResult.Results);
                //    token = queryResult.ContinuationToken;
                //} while (token != null);

                var filter = "PartitionKey ne ''";
                var query = _azureTableClient.GetTableClient(TableName).Query<RegisterUserEntity>(filter: filter);

                List<RegisterUser> registerTableEntityList = new List<RegisterUser>();

                foreach (RegisterUserEntity item in query)
                {
                    registerTableEntityList.Add(new RegisterUser()
                    {
                        FirstName = item.FirstName,
                        Id = item.Id,
                        LastName = item.LastName,
                        Password = item.Password,
                        Role = item.Role,
                        Username = item.Username
                    });
                }

                return await Task.FromResult(registerTableEntityList.ToList());

            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                return null;
            }

        }
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
        public async Task<bool> Update(RegisterUser record)
        {
            RegisterUserEntity cretareduser = new RegisterUserEntity()
            {
                PartitionKey = record.Id,
                RowKey = record.Id,
                FirstName = record.FirstName,
                Id = record.Id,
                LastName = record.LastName,
                Password = record.Password,
                Role = record.Role,
                Username = record.Username,
                ETag = new Azure.ETag("*")
            };

            Response response = null;
            try
            {
                response = await _azureTableClient.GetTableClient(TableName).UpdateEntityAsync(cretareduser, cretareduser.ETag);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
                //  response = await _azureTableClient.GetTableClient(TableName).UpdateEntityAsync(createdUser, createdUser.ETag);
            }
            //Console.WriteLine(response);
            return true;
        }

        public Task<bool> Check(string username, string password)
        {
            Response<RegisterUserEntity> response = null;
            RegisterUserEntity deviceTableEntityList = new RegisterUserEntity();

             var filter = $"Username eq '{username}' and Password eq '{password}'";
             var query = _azureTableClient.GetTableClient(TableName).Query<RegisterUserEntity>(filter: filter);
            bool IsNotEmpty = false;

                foreach (RegisterUserEntity item in query)
                {
                    deviceTableEntityList = item;
                IsNotEmpty = true;
                }

                if(!IsNotEmpty)
                return Task.FromResult(false);

            return Task.FromResult(true);
        }
    }
    }


