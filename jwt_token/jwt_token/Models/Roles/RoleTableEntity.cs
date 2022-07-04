using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jwt_token.Models
{
    public class RoleTableEntity : ITableEntity
    {
      
        public ETag ETag { get; set; } = new ETag ("*");
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public string Name { get; set; }

        public RoleTableEntity()
        {
                
        }

        public RoleTableEntity( Role role)
        {
            ETag = new ETag ("*");
            PartitionKey = role.Id.ToString();
            RowKey = role.Id.ToString();
            Timestamp = DateTime.Now;
            Name = role.Name;
        }
    }
}
