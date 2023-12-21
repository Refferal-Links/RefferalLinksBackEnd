using MayNghien.Models.Request.Base;
using MayNghien.Models.Response.Base;
using Microsoft.AspNetCore.Identity;
using RefferalLinks.DAL.Models.Entity;
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
		public Task<AppResponse<List<UserModel>>> GetAllUser();
		public Task<AppResponse<string>> ResetPassWordUser(string Id);
        public Task<AppResponse<string>> CreateUser(UserModel model);
        public Task<AppResponse<string>> DeleteUser(string id);
        public Task<AppResponse<UserModel>> GetUser(string email);
        Task<AppResponse<SearchResponse<UserModel>>> Search(SearchRequest request);
        public  Task<AppResponse<string>> StatusChange(UserModel request);
        public Task<AppResponse<List<UserModel>>> GetAllSale();
        Task<AppResponse<List<UserModel>>> GetAllCSKH();

        AppResponse<UserModel> Edit(UserModel request);
    }
}
