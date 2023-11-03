using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maynghien.Common.Models;
using MayNghien.Common.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RefferalLinks.DAL.Models.Entity;

namespace RefferalLinks.DAL.Models.Context
{
	public class RefferalLinksDbContext : BaseContext<AspNetUser>
	{
		public RefferalLinksDbContext()
		{

		}
		public RefferalLinksDbContext(DbContextOptions options) : base(options)
		{

		}
        public DbSet<TeamManagement> TeamManagement { get;set; }


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
