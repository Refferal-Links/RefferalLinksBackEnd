using MayNghien.Common.Models;
using RefferalLinks.Common.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefferalLinks.Models.Dto
{
    public class StatisticalStatusDto : BaseDto
    {
       
        public string? BranchName { get; set; }
        public Guid? BranchId { get; set; }
        public string? TeamName { get; set; }
        public string? Sale { get; set; }
        public string? CampaignName { get;set; }
        public StatusCustomerLink? Status { get; set; }
        public Guid? TeamId { get; set; }
        public int? Pending { get; set; }
        public int? Approved { get; set; }
        public int? Rejected { get; set; }
        public int? Cancel { get; set; }
        public int? Total { get; set; }
        public string? TotalString { get; set; }
        
        public Guid? BankId { get; set; }
        public Guid? CampaignId { get; set; }
        public string? CreateOn { get; set; }
        public string? ModifiedOn { get; set;}

        public double? TotalLead { get; set; }

    }
}
