using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Common.Models.Entity;

namespace RefferalLinks.DAL.Models.Entity
{
	public class Campaign : BaseEntity
	{
		public string Name { get; set; }

		public bool IsActive { get; set; }
	}
}
