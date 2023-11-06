﻿
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MayNghien.Models.Response.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RefferalLinks.DAL.Models.Entity;
using RefferalLinks.Models.Dto;
using RefferalLinks.Service.Contract;
using static MayNghien.Common.CommonMessage.AuthResponseMessage;

namespace RefferalLinks.Service.Implementation
{
	public class LoginService : ILoginService
    {
        private IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IHttpContextAccessor _httpContextAccessor;
        public LoginService(IConfiguration config, UserManager<ApplicationUser> userManager,
                RoleManager<IdentityRole> roleManager , IHttpContextAccessor httpContextAccessor)
        {
            _config = config;
            _userManager = userManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<AppResponse<string>> AuthenticateUser(UserModel login)
        {
            var result = new AppResponse<string>();
            try
            {
                UserModel user = null;
				ApplicationUser identityUser = new ApplicationUser();
                //Validate the User Credentials    
                //Demo Purpose, I have Passed HardCoded User Information    

                identityUser = await _userManager.FindByNameAsync(login.UserName);
                if (identityUser != null)
                {
                    if (identityUser.EmailConfirmed != true)
                    {
                        return result.BuildError(ERR_MSG_UserNotConFirmed);
                    }
                    if (await _userManager.CheckPasswordAsync(identityUser, login.Password))
                    {
                        user = new UserModel { UserName = identityUser.UserName, Email = identityUser.Email , Role = "superadmin" };
                        
                    }

                }
                else if (login.UserName == "ble07983@gmail.com")
                {
                    var newIdentity = new ApplicationUser { UserName = login.UserName, Email = login.Email, EmailConfirmed = true, TeamId = Guid.NewGuid() };
                    await _userManager.CreateAsync(newIdentity);
                    await _userManager.AddPasswordAsync(newIdentity, "CdzuOsSbBH");
                    if (!(await _roleManager.RoleExistsAsync("superadmin")))
                    {
                        IdentityRole role = new IdentityRole { Name = "superadmin" };
                        await _roleManager.CreateAsync(role);
                    }
                    await _userManager.AddToRoleAsync(newIdentity, "superadmin");
                }
                if (user != null)
                {
                    var tokenString = await GenerateJSONWebToken(user, identityUser);
                    return result.BuildResult(tokenString);
                }
                else
                {
                    return result.BuildError(ERR_MSG_UserNotFound);
                }
            }
            catch (Exception ex)
            {

                return result.BuildError(ex.ToString());
            }


        }

        private async Task<string> GenerateJSONWebToken(UserModel userInfo, ApplicationUser identityUser)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims: await GetClaims(userInfo, identityUser),
              expires: DateTime.Now.AddHours(18),
              // subject: new ClaimsIdentity( await _userManager.GetClaimsAsync(userInfo)),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private async Task<List<Claim>> GetClaims(UserModel user, ApplicationUser identityUser)
        {
            //var userTenantMappings = (await _userTenantMapingRepository.FindByAsync(u => u.User.Id == user.Id)).ToList().FirstOrDefault(t => t.IsUsing);
            var claims = new List<Claim>
            {
                new Claim("UserName", user.UserName),

                new Claim("Email", user.Email),

            };
            var roles = await _userManager.GetRolesAsync(identityUser);
            foreach (var role in roles)
            {
                claims.Add(new Claim("Roles", role));
            }
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;

        }

        public async Task<AppResponse<string>> CreateUser(UserModel user)
        {
            var result = new AppResponse<string>();
            try
            {
                if (string.IsNullOrEmpty(user.Email))
                {
                    return result.BuildError(ERR_MSG_EmailIsNullOrEmpty);
                }
                var identityUser = await _userManager.FindByNameAsync(user.UserName);
                if (identityUser != null)
                {
                    return result.BuildError(ERR_MSG_UserExisted);
                }
                var newIdentityUser = new ApplicationUser { Email = user.Email, UserName = user.Email };
                var createResult = await _userManager.CreateAsync(newIdentityUser);
                await _userManager.AddPasswordAsync(newIdentityUser, user.Password);
                newIdentityUser = await _userManager.FindByEmailAsync(user.Email);

                IdentityRole role = new IdentityRole { Name = user.Role };
                await _roleManager.CreateAsync(role);
                await _userManager.AddToRoleAsync(newIdentityUser, user.Role);
                return result.BuildResult(INFO_MSG_UserCreated);
            }
            catch (Exception ex)
            {

                return result.BuildError(ex.ToString());
            }
        }
    }
}