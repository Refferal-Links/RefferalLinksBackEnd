using MayNghien.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefferalLinks.Models.Dto
{
    public class CustomerLinkDto : BaseDto
    {
        public Guid CustomerId { get; set; }
        public string Url { get; set; }

        public Guid LinkTemplateId { get; set; }

    }
}
