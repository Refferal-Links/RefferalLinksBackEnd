using System.ComponentModel.DataAnnotations.Schema;
using MayNghien.Common.Models.Entity;

namespace RefferalLinks.DAL.Models.Entity
{
    public class Team : BaseEntity
    {
        public string name { get; set; }

        public string? Type { get; set; }

        [ForeignKey("Branch")]
        public Guid? BranchId { get; set; }
        [ForeignKey("BranchId")]
        public Branch? Branch { get; set; }
    }
}
