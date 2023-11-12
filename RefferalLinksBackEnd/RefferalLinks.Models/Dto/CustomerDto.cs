using MayNghien.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefferalLinks.Models.Dto
{
    public class CustomerDto : BaseDto
    {
        public string Name { get; set; }
        public bool Passport { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Cccd { get; set; }

        public string RefferalCode { get; set; }

        public string NameProvice { get; set; }
        public Guid ProvinceId { get; set; }

        public List<BankDto>? Banks { get; set; }
    }
}
