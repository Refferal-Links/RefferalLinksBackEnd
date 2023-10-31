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
        [ForeignKey("TeamManagement")]
        public Guid TeamManagementId { get; set; }
        [ForeignKey("TeamManagementId")]
        public TeamManagement TeamManagement { get; set; }

    }
}
