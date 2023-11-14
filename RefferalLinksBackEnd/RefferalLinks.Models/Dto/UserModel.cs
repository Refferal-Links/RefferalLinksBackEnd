using MayNghien.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefferalLinks.Models.Dto
{
  public class UserModel : BaseDto
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public Guid? TeamId { get; set; }
        public string? Reffercode { get; set; }
        public string? TPbank { get; set; }
        public string? LockoutEnabled { get; set; }
    }
}
