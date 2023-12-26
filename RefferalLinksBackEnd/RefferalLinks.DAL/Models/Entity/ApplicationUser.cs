using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefferalLinks.DAL.Models.Entity
{
    public class ApplicationUser : IdentityUser
    {
        
        public Guid? TeamId { get; set; }
        public string? RefferalCode { get; set; }

        public string? TpBank { get; set; }
        public string? User { get; set; }

        public Guid? BranchId { get; set; }
    }
}
