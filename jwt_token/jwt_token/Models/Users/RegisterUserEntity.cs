using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jwt_token.Models
{
    public class RegisterUserEntity : ITableEntity
    {
      
        public ETag ETag { get; set; } = new ETag ("*");
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public string Token { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public RegisterUserEntity()
        {
                
        }

        public RegisterUserEntity( string partitionKey, string rowKey, DateTimeOffset? timestamp, string id, string firstName, string lastName, string username, string password, string role)
        {
            ETag = new ETag ("*");
            PartitionKey = partitionKey;
            RowKey = rowKey;
            Timestamp = timestamp;
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Password = password;
            Role = role;
           
        }
    }
}
