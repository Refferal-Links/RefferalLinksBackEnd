using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefferalLinks.Models.Dto
{
    public class ChangePassword
    {
        public string Email { get; set; }
        public string InitialPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
