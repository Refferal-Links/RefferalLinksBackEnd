using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace RefferalLinks.DAL.Models.Context
{
	public class RefferalLinksDbContext : BaseContext
	{
		public RefferalLinksDbContext()
		{

		}
		public RefferalLinksDbContext(DbContextOptions options) : base(options)
		{

		}
	}
}
