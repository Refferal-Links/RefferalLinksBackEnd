using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefferalLinks.Models.Dto
{
    public class LoginResult
    {
        public string? UserName { get; set; }
        public string? RefferalCode { get; set; }
        public string? TpBank { get; set; }
        public List<string>? Roles { get; set; }
        public string Token { get; set; }
        public Guid? TeamId { get; set; }
    }
}
