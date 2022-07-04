using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jwt_token.Models
{
    public class Role 
    {
        //guid id, string Name, List<Permission> Permissions
        public Guid  Id { get; set; }
        public string Name { get; set; }

        public ETag ETag { get; set; } = new ETag("*");

        public Role()
        {
        }
        //public List<Permission> Permissions { get; set; } = null;
    }
}
