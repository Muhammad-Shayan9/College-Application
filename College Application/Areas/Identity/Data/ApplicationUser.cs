using Microsoft.AspNetCore.Identity;
using System;

namespace College_Application.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {
        // Extra properties
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}