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
        public string? IsActive { get; set; }
        public Guid BankId { get; set; }
        public string? BankName { get; set; }
        public Guid CampaignId { get; set; }
        public string? CampaignName { get; set;}
        public string? InstructionsLink { get; set; }
        public string? Note { get; set; }
    }
}
