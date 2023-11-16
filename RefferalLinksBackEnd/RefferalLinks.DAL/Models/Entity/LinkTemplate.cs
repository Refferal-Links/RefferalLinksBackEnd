using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Common.Models.Entity;

namespace RefferalLinks.DAL.Models.Entity
{
    public class LinkTemplate : BaseEntity
    {
        public string Url { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("Bank")]
        public Guid BankId { get; set; }
        [ForeignKey("BankId")]
        public Bank Bank { get; set;}

        [ForeignKey("Campaign")]
        public Guid CampaignId { get; set; }
        [ForeignKey("CampaignId")]
        public Campaign Campaign { get; set; }
    }
}
