using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Common.Models;

namespace RefferalLinks.Models.Dto
{
	public class BankDto : BaseDto
	{
		public string Name { get; set; }
		public List<CustomerLinkDto>? CustomerLinks { get; set; }
	}
}
