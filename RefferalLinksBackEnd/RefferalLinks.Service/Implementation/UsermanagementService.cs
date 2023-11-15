using AutoMapper;
using LinqKit;
using MayNghien.Models.Request.Base;
using MayNghien.Models.Response.Base;
using Microsoft.AspNetCore.Identity;
using RefferalLinks.DAL.Contract;
using RefferalLinks.DAL.Models.Context;
using RefferalLinks.DAL.Models.Entity;
using RefferalLinks.Models.Dto;
using RefferalLinks.Service.Contract;
using static MayNghien.Common.CommonMessage.AuthResponseMessage;
using static Maynghien.Common.Helpers.SearchHelper;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using RefferalLinks.DAL.Implementation;
using System.Text.Json;

namespace RefferalLinks.Service.Implementation
{
	public class UsermanagementService : IUsermanagementService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IUserespository _userRepository;
        private readonly ITeamRespository _teamRespository;
        public UsermanagementService(UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager , RefferalLinksDbContext context , IMapper mapper , IUserespository userespository , ITeamRespository teamRespository) {

            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _userRepository = userespository;
            _teamRespository = teamRespository;
        }
		public async Task<AppResponse<List<UserModel>>> GetAllUser()
		{
			var result = new AppResponse<List<UserModel>>();
			try
			{
				List<Filter> Filters = new List<Filter>();
				var query =  BuildFilterExpression(Filters);
				var users = _userRepository.FindByPredicate(query);
				var UserList = users.ToList();
				var dtoList = _mapper.Map<List<UserModel>>(UserList);
               
           
                if (dtoList != null && dtoList.Count > 0)
                {
                    for (int i = 0; i < UserList.Count; i++)
                    {
                        var dtouser = dtoList[i];
                        
                        var identityUser = UserList[i];
                        if (UserList[i].LockoutEnabled == true ) {
                            dtouser.LockoutEnabled = "Normal";
                        }
                        else
                        {
                            dtouser.LockoutEnabled = "Banned";
                        }
                        dtouser.Role = (await _userManager.GetRolesAsync(identityUser)).First();
                  
                    }
                }
                return result.BuildResult(dtoList);
			}
			catch (Exception ex)
			{

				return result.BuildError(ex.ToString());
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

                if(user.Role == "sale" || user.Role == "teamleader")
                {
                    var idteam =  _teamRespository.Get((Guid) user.TeamId);
                    if(user.TeamId == null ||  idteam == null )
                    {
                        return result.BuildError("Vui long nhap dung teamId");
                    }
                    else
                    {
                        var newIdentityUserSale = new ApplicationUser { Email = user.Email, UserName = user.Email, TeamId = user.Id };
                        if(user.Role == "sale")
                        {
                            newIdentityUserSale.RefferalCode = user.Reffercode;
                            newIdentityUserSale.TpBank = user.TPbank;
                        }

                        var createResultSale = await _userManager.CreateAsync(newIdentityUserSale);
                        await _userManager.AddPasswordAsync(newIdentityUserSale, user.Password);
                        if (!(await _roleManager.RoleExistsAsync(user.Role)))
                        {
                            IdentityRole role = new IdentityRole { Name = user.Role };
                            await _roleManager.CreateAsync(role);
                        }
                        await _userManager.AddToRoleAsync(newIdentityUserSale, user.Role);
                        newIdentityUserSale = await _userManager.FindByEmailAsync(user.Email);
                        return result.BuildResult(INFO_MSG_UserCreated);

                    }

                }
                var newIdentityUser = new ApplicationUser { Email = user.Email, UserName = user.Email , TeamId = null };

                var createResult = await _userManager.CreateAsync(newIdentityUser);
                await _userManager.AddPasswordAsync(newIdentityUser, user.Password);
				if (!(await _roleManager.RoleExistsAsync(user.Role)))
				{
					IdentityRole role = new IdentityRole { Name = user.Role };
					await _roleManager.CreateAsync(role);
				}
				await _userManager.AddToRoleAsync(newIdentityUser, user.Role);
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

				ApplicationUser identityUser = new ApplicationUser();

                identityUser = await _userManager.FindByIdAsync(id);
                if (identityUser != null)
                {
                    if (await _userManager.IsInRoleAsync(identityUser, "tenant"))
                    {
						//var userRoles = await _userManager.GetRolesAsync(identityUser);
      //                  foreach(var role in userRoles)
      //                  {
						//	await _userManager.RemoveFromRoleAsync(identityUser, role);
						//}
						var user = _userRepository.FindUser(id);
                        await _userManager.DeleteAsync(user);
                    }
                    else
                    {
						var user = _userRepository.FindUser(id);
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
		public async Task<AppResponse<SearchResponse<UserModel>>> Search(SearchRequest request)
		{
			var result = new AppResponse<SearchResponse<UserModel>> ();
			try
			{
				var query = BuildFilterExpression(request.Filters);
				var numOfRecords = _userRepository.CountRecordsByPredicate(query);

				var users = _userRepository.FindByPredicate(query).ToList();

                for(int i=0;i<users.Count; i++)
                {
                    if((await _userManager.GetRolesAsync(users[i])).First() == "superadmin")
                    {
                        users.Remove(users[i]);
                        i--;
                    }
                }

				int pageIndex = request.PageIndex ?? 1;
				int pageSize = request.PageSize ?? 1;
				int startIndex = (pageIndex - 1) * (int)pageSize;
				var UserList = users.Skip(startIndex).Take(pageSize).ToList();
                var dtoList = UserList.Select(x => new UserModel
                {
                    Email = x.Email,
                    UserName = x.UserName,
                    Id = Guid.Parse(x.Id)
                }).ToList();
				if (dtoList != null && dtoList.Count > 0)
				{
					for (int i = 0; i < UserList.Count; i++)
					{
						var dtouser = dtoList[i];
						var identityUser = UserList[i];
						dtouser.Role = (await _userManager.GetRolesAsync(identityUser)).First();
					}
				}
				var searchUserResult = new SearchResponse<UserModel>
				{
					TotalRows = numOfRecords,
					TotalPages = CalculateNumOfPages(numOfRecords, pageSize),
					CurrentPage = pageIndex,
					Data = dtoList,
				};

				result.Data = searchUserResult;
				result.IsSuccess = true;

				return result;

			}
			catch (Exception ex)
			{

				return result.BuildError(ex.ToString());
			}
		}


		private ExpressionStarter<ApplicationUser> BuildFilterExpression(List<Filter> Filters)
		{
			try
			{
				var predicate = PredicateBuilder.New<ApplicationUser>(true);
				if (Filters != null)
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
				
				return predicate;
			}
			catch (Exception)
			{

				throw;
			}
		}


        public async Task< AppResponse<ApplicationUser> >StatusChange(string id)
        {
            var result = new AppResponse<ApplicationUser>();
            ApplicationUser userid = _userRepository.FindById(id);
            try
            {

                if (userid == null)
                {
                    return result.BuildError("Người dùng không tìm thấy");
                }
               
                DateTime LockoutEndnable = new DateTime(2024, 08, 01);
                var user = new ApplicationUser();
                await _userManager.SetLockoutEnabledAsync(userid, false);
                //user.LockoutEnd = LockoutEndnable;
                await _userManager.SetLockoutEndDateAsync(userid, LockoutEndnable);
                return result.BuildResult(user);
            }
            catch (Exception ex)
            {

                return result.BuildError(ex.ToString());
            }
        }
    }
}
