using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Common.Models;

namespace RefferalLinks.Models.Dto
{
    public class LinkTemplateDto : BaseDto
    {
        public string Url { get; set; }
        public bool IsActive { get; set; }
        public Guid BankId { get; set; }
        public Guid CampaignId { get; set; }
    }
}
