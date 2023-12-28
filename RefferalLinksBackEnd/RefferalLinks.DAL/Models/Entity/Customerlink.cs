using MayNghien.Common.Models.Entity;
using RefferalLinks.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefferalLinks.DAL.Models.Entity
{
    public class Customerlink : BaseEntity
    {
        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }

        public string? Url { get; set; }
        public StatusCustomerLink? Status { get; set; }
        public string? Note { get; set; }

        [ForeignKey("LinkTemplate")]
        public Guid? LinkTemplateId { get; set; }
        [ForeignKey("LinkTemplateId")]
        public LinkTemplate? LinkTemplate { get; set; }
    }
}
