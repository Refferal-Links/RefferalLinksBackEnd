using MayNghien.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefferalLinks.Models.Dto
{
   public class TeamDto : BaseDto
    {
        public string name { get; set; }

        public Guid? BranchId { get; set; }
        public string? NameBranch { get; set; }

        public string? Type { get; set;}
    }
}
