using MayNghien.Common.Models;
using Microsoft.EntityFrameworkCore.Query.Internal;
using RefferalLinks.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefferalLinks.Models.Dto
{
    public class CustomerLinkDto : BaseDto
    {
        public Guid? CustomerId { get; set; }
        public string? Url { get; set; }

        public Guid? LinkTemplateId { get; set; }
        public string? InforCustomer { get;set; }
        public string? Name { get; set; }
        public string? Passport { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? BankName { get; set; }
        public Guid? CampaignId { get; set; }   
        public Guid? BankId { get; set; }
        public string? CamPaignName { get; set; }
        public Guid? TeamId { get; set; }
        public string? UserName { get; set; }
        public string? TeamName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? RefferalCode { get; set; }
        public string? TpBank { get;set; }
        public string? Iduser {get; set; }
        public List<CustomerlinkImageDto>? ListCustomerlinkImage { get; set; }
        public StatusCustomerLink? Status { get; set; }
        public string? StatusText { get; set; }

        public string? CreateOn { get; set; }
        public string? ModifiedOn { get; set; }
        public string? Note { get; set; }
        public string? NoteCSKH { get; set; }

        public string? Image1 { get; set; }
        public string? Image2 { get; set; }
        public string? Image3 { get; set; }
        public string? Image4 { get; set; }

        public string? AproveFirst {  get; set; }
        public string? NvCSKH { get; set; }
        public string? CodeNVCSKH { get; set; }

        public string? SourceCustomer { get; set; }

        public bool? Watched { get; set; }

        public Guid? ProvinceId { get; set; }
        public string? ProvinceName { get; set; }
        public string? ExchangeLead { get; set; }
        public bool? CustomerCancel { get; set; }
    }
}
