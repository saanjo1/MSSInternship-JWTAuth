using Azure;
using Azure.Data.Tables;
using jwt_token.AzureRepo;
using jwt_token.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace jwt_token
{
    public class UserRoleRepository : IAzureRepo<Models.UserRoles>
    {
        public string ConnnectionString { get; set; }
        public string TableName { get; set; }
        public TableServiceClient _azureTableClient { get; set; }

        public UserRoleRepository(string connnectionString, string tableName)
        {
            ConnnectionString = connnectionString??throw new ArgumentNullException("ConnnectionString", "ConnnectionString IS NULL");
            TableName = tableName?? throw new ArgumentNullException("TableName", "TableName IS NULL");
            _azureTableClient = new TableServiceClient(ConnnectionString);
            _azureTableClient.CreateTableIfNotExists(TableName);
            
        }

        public async Task Create(UserRoles record)
        {
            var createdUser = new UserRoleTableEntity()
            {
                ETag = new Azure.ETag("*"),
                PartitionKey = record.UserId.ToString(),
                RowKey = record.RoleId.ToString(),
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
        //        RegisterUser = record.RegisterUser,
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

        public Task<bool> Update(UserRoles record)
        {
            throw new NotImplementedException();
        }

       

        Task<Models.UserRoles> IAzureRepo<Models.UserRoles>.Read(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserRoles>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Check(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}

