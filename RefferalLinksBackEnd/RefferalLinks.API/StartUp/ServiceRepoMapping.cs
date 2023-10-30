using RefferalLinks.DAL.Contract;
using RefferalLinks.DAL.Implementation;
using RefferalLinks.Service.Contract;
using RefferalLinks.Service.Implementation;

namespace RefferalLinks.API.StartUp
{
	public class ServiceRepoMapping
	{
		public ServiceRepoMapping() { }
		public void Mapping(WebApplicationBuilder builder)
		{
			builder.Services.AddScoped<IBankService, BankService>();


			builder.Services.AddScoped<IBankRepository, BankRepository>();
		}
	}
}
