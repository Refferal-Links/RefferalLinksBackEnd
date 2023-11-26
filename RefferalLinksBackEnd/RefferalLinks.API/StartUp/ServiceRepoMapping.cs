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
			builder.Services.AddScoped<IUserespository, UserRespository>();
		

			builder.Services.AddScoped<IUsermanagementService , UsermanagementService>();
			builder.Services.AddScoped<ITeamService, TeamService>();
			builder.Services.AddScoped<ILoginService, LoginService>();
			builder.Services.AddScoped<IBankService, BankService>();
			builder.Services.AddScoped<ICampaignService, CampaignService>();
			builder.Services.AddScoped<IProvinceService, ProvinceService>();
			builder.Services.AddScoped<ICustomerService, CustomerService>();
			builder.Services.AddScoped<ICustomerLinkService, CustomerLinkService>();
			builder.Services.AddScoped<ILinkTemplateService, LinkTemplateService>();


			builder.Services.AddScoped<IBankRepository, BankRepository>();
			builder.Services.AddScoped<ICampaignRepository, CampaignRepository>();
			builder.Services.AddScoped<IProvinceRepository, ProvinceRepository>();
			builder.Services.AddScoped<ICustomerRespository, CustomerRespository>();
			builder.Services.AddScoped<ICustomerLinkRepository, CustomerLinkRepository>();
	        builder.Services.AddScoped<ITeamRespository, TeamRespository>();
			builder.Services.AddScoped<ILinkTemplateRepository,LinkTempalteRepository>();
			builder.Services.AddScoped<ICustomerlinkImageRepository, CustomerlinkImageRepository>();
		}
	}
}
