using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RefferalLinks.DAL.Models.Entity;

namespace RefferalLinks.DAL.Models.Context
{
	public class RefferalLinksDbContext : BaseContext<ApplicationUser>
	{
		public RefferalLinksDbContext()
		{

		}
		public RefferalLinksDbContext(DbContextOptions options) : base(options)
		{

		}
        public DbSet<Team> Team{ get;set; }
		public DbSet<Bank> Bank { get;set; }
		public DbSet<Province> Province { get;set; }
		public DbSet<Campaign> Campaign { get;set; }
		public DbSet<Customer> Customer { get;set; }
		public DbSet<LinkTemplate> LinkTemplate { get;set; }
		public DbSet<Customerlink> Customerlink { get;set; }
		public DbSet<CustomerLinkImage> CustomerlinkImage { get;set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				var appSetting = JsonConvert.DeserializeObject<AppSetting>(File.ReadAllText("appsettings.json"));
				optionsBuilder.UseSqlServer(appSetting.ConnectionString);
			}


		}
	}
}
