using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspModular.Data.Identity
{
    public class UserRole : IdentityUserRole<long>
    {
        public override long UserId { get; set; }

        public User User { get; set; }

        public override long RoleId { get; set; }

        public Role Role { get; set; }
    }
}
