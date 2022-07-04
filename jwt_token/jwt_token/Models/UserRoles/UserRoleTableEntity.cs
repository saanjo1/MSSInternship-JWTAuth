using Azure;
using Azure.Data.Tables;
using System;

namespace jwt_token.Models
{
    public class UserRoleTableEntity : ITableEntity
    {
      
        public ETag ETag { get; set; } = new ETag ("*");
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }


        public UserRoleTableEntity()
        {
                
        }

        public UserRoleTableEntity( UserRoles role)
        {
            ETag = new ETag ("*");
            PartitionKey = role.UserId.ToString();
            RowKey = role.RoleId.ToString();
            Timestamp = DateTime.Now;
        }
    }
}
