using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Common.Models.Entity;

namespace RefferalLinks.DAL.Models.Entity
{
    public class CustomerLinkImage : BaseEntity
    {
        public string LinkImage { get; set; }

        [ForeignKey("CustomerLink")]
        public Guid CustomerLinkId { get; set; }
        [ForeignKey("CustomerLinkId")]
        public Customerlink? Customerlink { get; set; }
    }
}
