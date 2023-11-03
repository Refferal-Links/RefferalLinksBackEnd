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
			builder.Services.AddScoped<ITeamRespository, TeamRespository>();

			builder.Services.AddScoped<IUsermanagementService , UsermanagementService>();
			builder.Services.AddScoped<ITeamService, TeamService>();
			builder.Services.AddScoped<ILoginService, LoginService>();

			builder.Services.AddTransient<RestSharp.IHttpResponse, RestSharp.HttpResponse>();
		}
	}
}
