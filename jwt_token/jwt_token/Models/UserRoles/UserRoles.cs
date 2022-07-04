using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jwt_token.Models
{
    public class UserRoles
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

    }
}
