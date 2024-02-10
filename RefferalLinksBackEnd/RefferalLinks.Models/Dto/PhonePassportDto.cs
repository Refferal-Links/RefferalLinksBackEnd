using MayNghien.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefferalLinks.Models.Dto
{
    public class PhonePassportDto : BaseDto
    {
        public string PhoneNumber { get; set; }
        public string Passport { get; set; }

    }
}
