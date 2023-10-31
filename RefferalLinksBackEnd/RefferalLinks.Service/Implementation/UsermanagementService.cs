using AutoMapper;
using MayNghien.Models.Request.Base;
using MayNghien.Models.Response.Base;
using MayNghien.Common.Helpers;
using Microsoft.AspNetCore.Identity;
using RefferalLinks.DAL.Contract;
using RefferalLinks.DAL.Implementation;
using RefferalLinks.DAL.Models.Context;
using RefferalLinks.Models.Dto;
using RefferalLinks.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqKit;
using static MayNghien.Common.CommonMessage.AuthResponseMessage;
namespace RefferalLinks.Service.Implementation
{
    public class UsermanagementService : IUsermanagementService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private RefferalLinksDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserespository _userRepository;

        public UsermanagementService(UserManager<IdentityUser> userManager , RoleManager<IdentityRole> roleManager , RefferalLinksDbContext context , IMapper mapper , IUserespository userespository) {

            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _mapper = mapper;
            _userRepository = userespository;
        }
        public AppResponse<List<IdentityUser>> GetAllUser()
        {


            var result = new AppResponse<List<IdentityUser>>();
            try
            {
                var list = _userRepository.GetAll();

                result.IsSuccess = true;
                result.Data = list;
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
                return result;
            }
        }
        public async Task<AppResponse<string>> ResetPassWordUser(string Id)
        {
            var result = new AppResponse<string>();
            try
            {
                var user = _userRepository.FindUser(Id.ToString());
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, "dungroi");
                result.IsSuccess = true;
                result.Data = "dungroi";
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
                return result;
            }
        }
        public async Task<AppResponse<string>> Password(UserModel user)
        {
            var result = new AppResponse<string>();
            try
            {
                var userid = _userRepository.FindById(user.Id.ToString());
                await _userManager.RemovePasswordAsync(userid);
                await _userManager.AddPasswordAsync(userid, user.Password);
                result.IsSuccess = true;
                result.Data = user.Password;
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
                return result;
            }
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
                var newIdentityUser = new IdentityUser { Email = user.Email, UserName = user.Email };

                var createResult = await _userManager.CreateAsync(newIdentityUser);
                await _userManager.AddPasswordAsync(newIdentityUser, user.Password);

                newIdentityUser = await _userManager.FindByEmailAsync(user.Email);
                return result.BuildResult(INFO_MSG_UserCreated);
            }
            catch (Exception ex)
            {

                return result.BuildError(ex.ToString());
            }
        }


        public async Task<AppResponse<string>> DeleteUser(string id)
        {
            var result = new AppResponse<string>();
            try
            {

                IdentityUser identityUser = new IdentityUser();

                identityUser = await _userManager.FindByIdAsync(id);
                if (identityUser != null)
                {
                    if (await _userManager.IsInRoleAsync(identityUser, "tenant"))
                    {

                        var user = _context.Users.FirstOrDefault(x => x.Id == id);
                        _context.Users.Remove(user);
                        //_userManager.DeleteAsync(user);
                    }
                    else
                    {
                        var user = _context.Users.FirstOrDefault(x => x.Id == id);
                        await _userManager.DeleteAsync(user);

                    }

                }
                return result.BuildResult(INFO_MSG_UserDeleted);
            }
            catch (Exception ex)
            {

                return result.BuildError(ex.ToString());
            }
        }

        public async Task<AppResponse<string>> EditUser(UserModel model)
        {
            var result = new AppResponse<string>();
            if (model.Email == null)
            {
                return result.BuildError(ERR_MSG_EmailIsNullOrEmpty);
            }
            try
            {
                var identityUser = await _userManager.FindByIdAsync(model.Email);

                if (identityUser != null)
                {

                    //model.Id = identityUser.Id;
                    model.UserName = identityUser.UserName;
                    model.Email = identityUser.Email;
                    //model.LockoutEnabled  =  identityUser.LockoutEnabled ;


                }
                return result.BuildResult("ok");
            }
            catch (Exception ex)
            {

                return result.BuildError(ex.ToString());
            }
        }

        public async Task<AppResponse<UserModel>> GetUser(string Id)
        {
            var result = new AppResponse<UserModel>();
            try
            {
                List<Filter> Filters = new List<Filter>();
                var query = BuildFilterExpression(Filters);

                //var identityUser = _userRepository.FindById(id);
                var identityUser = _userRepository.FindById(Id);

                if (identityUser == null)
                {
                    return result.BuildError("User not found");
                }
                var dtouser = _mapper.Map<UserModel>(identityUser);

                dtouser.Role = (await _userManager.GetRolesAsync(identityUser)).First();

                return result.BuildResult(dtouser);
            }
            catch (Exception ex)
            {

                return result.BuildError(ex.ToString());
            }
        }


        // Get identityuser
        public async Task<AppResponse<IdentityUser>> GetUserIdentity(string Id)
        {
            var result = new AppResponse<IdentityUser>();
            try
            {
                List<Filter> Filters = new List<Filter>();
                var query = BuildFilterExpression(Filters);

                //var identityUser = _userRepository.FindById(id);
                var identityUser = _userRepository.FindById(Id);

                if (identityUser == null)
                {
                    return result.BuildError("User not found");
                }
                var dtouser = _mapper.Map<IdentityUser>(identityUser);


                return result.BuildResult(dtouser);
            }
            catch (Exception ex)
            {

                return result.BuildError(ex.ToString());
            }
        }
        private ExpressionStarter<IdentityUser> BuildFilterExpression(IList<Filter>? Filters)
        {
            try
            {
                var predicate = PredicateBuilder.New<IdentityUser>(true);
                if (Filters != null)
                {

                    foreach (var filter in Filters)
                    {
                        switch (filter.FieldName)
                        {
                            case "userName":
                                predicate = predicate.And(m => m.UserName.Equals(filter.Value));
                                break;

                            default:
                                break;
                        }
                    }
                }
                return predicate;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
