using AspModular.Data.Models.Abstractions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspModular.Data.Identity
{
    public class Role : IdentityRole<long>, IEntityWithTypedId<long>
    {
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string IPAddress { get; set; }
    }
}
