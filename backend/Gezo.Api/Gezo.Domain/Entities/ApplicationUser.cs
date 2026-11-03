using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Domain.Entities
{
    public class ApplicationUser: IdentityUser
    {
        public string? FullName { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? Governorate { get; set; }
        public int? PostalCode { get; set; }

    }
}
