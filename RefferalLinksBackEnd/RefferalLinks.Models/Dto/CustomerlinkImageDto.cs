using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Common.Models;

namespace RefferalLinks.Models.Dto
{
    public class CustomerlinkImageDto : BaseDto
    {
        public string LinkImage { get; set; }

        public Guid? CustomerLinkId { get; set; }
    }
}
