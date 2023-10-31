using MayNghien.Models.Response.Base;
using Microsoft.AspNetCore.Identity;
using RefferalLinks.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefferalLinks.Service.Contract
{
    public interface IUsermanagementService
    {
        public AppResponse<List<IdentityUser>> GetAllUser();
        public Task<AppResponse<string>> ResetPassWordUser(string Id);
        public Task<AppResponse<string>> Password(UserModel user);
        public Task<AppResponse<string>> CreateUser(UserModel model);
        public Task<AppResponse<string>> DeleteUser(string id);
        public Task<AppResponse<UserModel>> GetUser(string email);
        public Task<AppResponse<IdentityUser>> GetUserIdentity(string Id);

    }
}
