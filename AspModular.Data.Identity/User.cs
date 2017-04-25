using AspModular.Data.Models.Abstractions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspModular.Data.Identity
{
    public class User : IdentityUser<long>, IEntityWithTypedId<long>
    {
        public Guid UserGuid { get; set; }

        public string FullName { get; set; }

        public bool IsDeleted { get; set; }

        public bool Gender { get; set; }

        public string Avatar { get; set; }

        public string Birthday { get; set; }

        public DateTimeOffset CreatedOn { get; set; }
    }
}
