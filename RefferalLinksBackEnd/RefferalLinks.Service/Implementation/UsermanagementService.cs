﻿using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using LinqKit;
using MayNghien.Common.Helpers;
using MayNghien.Models.Request.Base;
using MayNghien.Models.Response.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using RefferalLinks.DAL.Contract;
using RefferalLinks.DAL.Models.Context;
using RefferalLinks.DAL.Models.Entity;
using RefferalLinks.Models.Dto;
using RefferalLinks.Service.Contract;
using static Maynghien.Common.Helpers.SearchHelper;
using static MayNghien.Common.CommonMessage.AuthResponseMessage;

namespace RefferalLinks.Service.Implementation
{
    public class UsermanagementService : IUsermanagementService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IUserespository _userRepository;
        private readonly ITeamRespository _teamRespository;
        private IHttpContextAccessor _httpContextAccessor;
      
        public UsermanagementService(UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager , RefferalLinksDbContext context , IMapper mapper , IUserespository userespository , ITeamRespository teamRespository ,
           IHttpContextAccessor httpContextAccessor
            ) {

            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _userRepository = userespository;
            _teamRespository = teamRespository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AppResponse<List<UserModel>>> GetAllSale()
        {
            var result = new AppResponse<List<UserModel>>();
            try
            {
             

                List<Filter> Filters = new List<Filter>();
                var query = await BuildFilterExpression2(Filters);
                var users = _userRepository.FindByPredicate(query);
                var UserList = users.ToList();
                var dtoList = UserList.Select(x =>
                {
                    var user = new UserModel
                    {
                        Email = x.Email,
                        UserName = x.UserName,
                        Id = Guid.Parse(x.Id),
                        LockoutEnabled = x.LockoutEnabled ? "hoạt động" : "cấm",
                        RefferalCode = x.RefferalCode ?? "",
                        TpBank = x.TpBank,

                    };
                    if (x.TeamId != null)
                    {
                        user.TeamName = _teamRespository.Get(x.TeamId.Value).name;
                    }
                    return user;
                }).ToList();
                if (dtoList != null && dtoList.Count > 0)
                {
                    for (int i = 0; i < UserList.Count; i++)
                    {
                        var dtouser = dtoList[i];
                
                        var identityUser = UserList[i];
                        if (UserList[i].LockoutEnabled == true)
                        {
                            dtouser.LockoutEnabled = "Hoạt động";
                        }
                        else
                        {
                            dtouser.LockoutEnabled = "cấm";
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

        public async Task<AppResponse<List<UserModel>>> GetAllCSKH()
        {
            var result = new AppResponse<List<UserModel>>();
            try
            {


                List<Filter> Filters = new List<Filter>();
                Filters.Add(new Filter
                {
                    FieldName = "Role",
                    Value = "CSKH"
                });
                var query = await BuildFilterExpression2(Filters);
                var users = _userRepository.FindByPredicate(query);
                var UserList = users.ToList();
                var dtoList = UserList.Select(x =>
                {
                    var user = new UserModel
                    {
                        Email = x.Email,
                        UserName = x.UserName,
                        Id = Guid.Parse(x.Id),
                        LockoutEnabled = x.LockoutEnabled ? "hoạt động" : "cấm",
                        RefferalCode = x.RefferalCode ?? "",
                        TpBank = x.TpBank,

                    };
                    if (x.TeamId != null)
                    {
                        user.TeamName = _teamRespository.Get(x.TeamId.Value).name;
                    }
                    return user;
                }).ToList();
                if (dtoList != null && dtoList.Count > 0)
                {
                    for (int i = 0; i < UserList.Count; i++)
                    {
                        var dtouser = dtoList[i];

                        var identityUser = UserList[i];
                        if (UserList[i].LockoutEnabled == true)
                        {
                            dtouser.LockoutEnabled = "Hoạt động";
                        }
                        else
                        {
                            dtouser.LockoutEnabled = "cấm";
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

        public async Task<AppResponse<List<UserModel>>> GetAllUser()
		{
			var result = new AppResponse<List<UserModel>>();
			try
			{
				List<Filter> Filters = new List<Filter>();
				var query =  BuildFilterExpression(Filters);
				var users = _userRepository.FindByPredicate(query);
				var UserList = users.ToList();
				


                var dtoList = UserList.Select(x =>
                {
                    var user = new UserModel
                    {
                        Email = x.Email,
                        UserName = x.UserName,
                        Id = Guid.Parse(x.Id),
                        LockoutEnabled = x.LockoutEnabled ? "hoạt động" : "cấm",
                        RefferalCode = x.RefferalCode ?? "",
                        TpBank = x.TpBank,
                        TeamId = x.TeamId,

                    };
                    if (x.TeamId != null)
                    {
                        user.TeamName = _teamRespository.Get(x.TeamId.Value).name;
                    }
                    return user;
                }).ToList();


                if (dtoList != null && dtoList.Count > 0)
                {
                    for (int i = 0; i < UserList.Count; i++)
                    {
                        var dtouser = dtoList[i];
                        
                        var identityUser = UserList[i];
                        if (UserList[i].LockoutEnabled == true ) {
                            dtouser.LockoutEnabled = "Hoạt động";
                        }
                        else
                        {
                            dtouser.LockoutEnabled = "Cấm";
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

                if(user.Role == "Sale" || user.Role == "Teamleader" || user.Role == "CSKH")
                {
                    var idteam =  _teamRespository.Get((Guid) user.TeamId);
                    if(user.TeamId == null ||  idteam == null )
                    {
                        return result.BuildError("Vui long nhap dung teamId");
                    }
                    else
                    {
                        var newIdentityUserSale = new ApplicationUser { Email = user.Email, UserName = user.Email, TeamId = user.TeamId };
                        if(user.Role == "Sale" || user.Role == "CSKH")
                        {
                            newIdentityUserSale.RefferalCode = user.RefferalCode;
                            newIdentityUserSale.TpBank = user.TpBank;
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
                    if (await _userManager.IsInRoleAsync(identityUser, "Sale"))
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

				var users = _userRepository.FindByPredicate(query);

                if (request.SortBy != null)
                {
                    users = addSort(users, request.SortBy);
                }
                var usersList = users.ToList();
                for (int i = 0; i < usersList.Count; i++)
                {
                    if ((await _userManager.GetRolesAsync(usersList[i])).First() == "superadmin")
                    {
                        usersList.Remove(usersList[i]);
                        i--;
                    }
                }

                int pageIndex = request.PageIndex ?? 1;
				int pageSize = request.PageSize ?? 1;
				int startIndex = (pageIndex - 1) * (int)pageSize;
				var UserList = usersList.Skip(startIndex).Take(pageSize).ToList();
                var dtoList = UserList.Select(x =>
                {
                    var user = new UserModel
                    {
                        Email = x.Email,
                        UserName = x.UserName,
                        Id = Guid.Parse(x.Id),
                        LockoutEnabled = x.LockoutEnabled ? "hoạt động" : "cấm",
                        RefferalCode = x.RefferalCode ?? "",
                        TpBank = x.TpBank,
                        
                    };
                    if(x.TeamId != null)
                    {
                        user.TeamName = _teamRespository.Get(x.TeamId.Value).name;
                    }
                    return user;
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

        private IQueryable<ApplicationUser> addSort(IQueryable<ApplicationUser> input, SortByInfo sortByInfo)
        {
            var result = input.AsQueryable();
            if (sortByInfo.FieldName == "userName")
            {
                if (sortByInfo.Ascending != null && sortByInfo.Ascending.Value)
                {
                    result = result.OrderBy(m => m.UserName);

                }
                else
                {
                    result = result.OrderByDescending(m => m.UserName);
                }
            }
            return result;
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
								predicate = predicate.And(m => m.UserName.Contains(filter.Value));
								break;
                            case "teamId":
                                predicate = predicate.And(m=>m.TeamId.Equals(Guid.Parse(filter.Value)));
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

        private async Task< ExpressionStarter<ApplicationUser>> BuildFilterExpression2(List<Filter> Filters)
        {
            try
            {
                var userRole = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;         
                var UserName = ClaimHelper.GetClainByName(_httpContextAccessor, "UserName");
                var iduser = await _userManager.FindByNameAsync(UserName);
                var usersWithRole = await _userManager.GetUsersInRoleAsync("Sale");
                if (Filters.Count > 0)
                {
                    var Role = Filters.Where(x => x.FieldName == "Role").First();
                    usersWithRole = await _userManager.GetUsersInRoleAsync(Role.Value);
                }
                var userIDs = usersWithRole.Select(u => u.Id).ToList();
                var userIDs2 = usersWithRole.Select(u => u.TeamId ).ToList();
                userIDs2.RemoveAll(user => user != iduser.TeamId);

                Expression<Func<ApplicationUser, bool>> predicate = u => true; 

                switch (userRole)
                {
                    case "Admin":
                    case "superadmin":                                      
                        predicate = u => userIDs.Contains(u.Id);
                        break;
                    case "Teamleader":
                        predicate = u => userIDs2.Contains(u.TeamId);
                            break;
                    case "Sale":
                        predicate = u => false;
                        break;
                    case "CSKH":
                        predicate = u => false;
                        break;
                    default:
                        break;
                }

                return predicate;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task< AppResponse<string> >StatusChange(UserModel request)
        {
            var result = new AppResponse<string>();
            ApplicationUser user = await _userManager.FindByIdAsync(request.Id.Value.ToString());
            try
            {

                if (user == null)
                {
                    return result.BuildError("Người dùng không tìm thấy");
                }
                if(user.LockoutEnabled == false)
                {
                    await _userManager.SetLockoutEnabledAsync(user, true);
                    user.LockoutEnd = null;
                    await _userManager.UpdateAsync(user);
                    return result.BuildResult("OK");
                }
                DateTimeOffset LockoutEndnable = DateTimeOffset.UtcNow.AddDays(30);             
                await _userManager.SetLockoutEnabledAsync(user, false);
                await _userManager.UpdateAsync(user);
                return result.BuildResult("OK");
            }
            catch (Exception ex)
            {

                return result.BuildError(ex.ToString());
            }
        }

        public AppResponse<UserModel> Edit(UserModel request)
        {
            var result = new AppResponse<UserModel>();
            try
            {
                var user = _userRepository.FindUser(request.Id.ToString());
                user.RefferalCode = request.RefferalCode;
                user.TpBank = request.TpBank;
                user.TeamId = request.TeamId;
                _userRepository.Edit(user);

                result.BuildResult(request);
            }
            catch(Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }
    }
}
