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
        public DateTime? AproveFirst {  get; set; }
        public int? CountAprove {  get; set; }
        public string? Note { get; set; }
        public bool? Watched { get; set; }
        public string? NoteCSKH { get; set; }
        public bool? CustomerCancel { get; set; }

        [ForeignKey("LinkTemplate")]
        public Guid? LinkTemplateId { get; set; }
        [ForeignKey("LinkTemplateId")]
        public LinkTemplate? LinkTemplate { get; set; }
    }
}
