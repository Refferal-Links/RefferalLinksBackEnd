using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RefferalLinks.API.StartUp;
using RefferalLinks.DAL.Models.Context;
using RefferalLinks.DAL.Models.Entity;
using RefferalLinks.Service.Mapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<RefferalLinksDbContext>(options =>
		options.UseSqlServer(
			builder.Configuration.GetConnectionString("DefaultConnection"))
		);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

new ServiceRepoMapping().Mapping(builder);

builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddCors(options =>
{
	// this defines a CORS policy called "default"
	options.AddPolicy("default", policy =>
	{
		policy.WithOrigins("http://localhost:5173", "*")
			.AllowAnyHeader()
			.AllowAnyOrigin()
			.AllowAnyMethod();
	});
});
//builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
//{
//	options.User.RequireUniqueEmail = false;
//	options.Password.RequireDigit = false;
//	options.Password.RequireLowercase = false;
//	options.Password.RequireUppercase = false;
//	options.Password.RequireNonAlphanumeric = false;
//})
//	.AddEntityFrameworkStores<RefferalLinksDbContext>()
//	.AddDefaultTokenProviders();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
	options.User.RequireUniqueEmail = false;
	options.Password.RequireDigit = false;
	options.Password.RequireLowercase = false;
	options.Password.RequireUppercase = false;
	options.Password.RequireNonAlphanumeric = false;
})
	.AddEntityFrameworkStores<RefferalLinksDbContext>()
	.AddDefaultTokenProviders();
builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddScoped<SignInManager<ApplicationUser>>();
builder.Services.AddAuthentication(opt =>
{
	opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.RequireHttpsMetadata = false;
	options.SaveToken = true;
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = false,
		ValidateAudience = false,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = builder.Configuration["JwtConfig:validIssuer"],
		ValidAudience = builder.Configuration["JwtConfig:validAudience"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
	};
});
builder.Services.AddAuthorization();
//builder.Services.AddAuthentication("Bearer").AddJwtBearer();
builder.Services.AddControllers().AddJsonOptions(x =>
				x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
	app.UseSwagger();
	app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();
app.UseCors(builder =>
{
	builder
	.AllowAnyOrigin()
	.AllowAnyMethod()
	.AllowAnyHeader();
});
app.UseAuthorization();

app.MapControllers();

app.Run("http://localhost:5001");
