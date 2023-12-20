using System.Data.Entity;
using AutoMapper;
using LinqKit;
using MayNghien.Common.Helpers;
using MayNghien.Models.Request.Base;
using MayNghien.Models.Response.Base;
using Microsoft.AspNetCore.Http;
using RefferalLinks.DAL.Contract;
using RefferalLinks.DAL.Models.Entity;
using RefferalLinks.Models.Dto;
using RefferalLinks.Service.Contract;
using static Maynghien.Common.Helpers.SearchHelper;

namespace RefferalLinks.Service.Implementation
{
	public class TeamService : ITeamService
    {
        private readonly  ITeamRespository _teamRespository;
        private readonly IMapper _mapper;
        private IHttpContextAccessor _httpContextAccessor;
        private IBranchRepository _branchRepository;

        public TeamService(ITeamRespository teamRespository , IMapper mapper , IHttpContextAccessor httpContextAccessor, IBranchRepository branchRepository) {
            _teamRespository = teamRespository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _branchRepository = branchRepository;
        }
        public AppResponse<TeamDto> CreateTeam(TeamDto request)
        {
            var result = new AppResponse<TeamDto>();
            try
            {
                var UserName = ClaimHelper.GetClainByName(_httpContextAccessor, "UserName");
                if (UserName == null)
                {
                    return result.BuildError("Cannot find Account by this user");
                }
                var branch = _branchRepository.FindBy(x=>x.Id == request.BranchId);
                if(branch.Count() == 0)
                {
                    return result.BuildError("Cannot find Branch");
                }
                var team = new Team();
                team = _mapper.Map<Team>(request);
                team.Id = Guid.NewGuid();
                team.CreatedBy = UserName;
                team.BranchId = request.BranchId.Value;
               _teamRespository.Add(team);

                request.Id = team.Id;
                result.IsSuccess = true;
                result.Data = request;
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
                return result;
            }
        }

        public AppResponse<string> DeleteTeam(Guid Id)
        {
            var result = new AppResponse<string>();
            try
            {
                var team = new Team();
                team = _teamRespository.Get(Id);
                team.IsDeleted = true;

                _teamRespository.Edit(team);

                result.IsSuccess = true;
                result.Data = "Delete Sucessfuly";
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + ":" + ex.StackTrace;
                return result;

            }
        }



        public AppResponse<TeamDto> EditTeam(TeamDto request)
        {
            var result = new AppResponse<TeamDto>();
            try
            {
                var UserName = ClaimHelper.GetClainByName(_httpContextAccessor, "UserName");
                var team = _teamRespository.Get(request.Id.Value);
                team.ModifiedOn = DateTime.UtcNow;
                team.Modifiedby = UserName;
                team.name = request.name;
                team.BranchId = request.BranchId.Value;
                //team.RefferalCode = request.RefferalCode;

				_teamRespository.Edit(team);

                result.IsSuccess = true;
                result.Data = request;
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
                return result;
            }
        }

        public AppResponse<List<TeamDto>> GetAllTeam()
        {
            var result = new AppResponse<List<TeamDto>>();
            //string userId = "";
            try
            {
                var query = _teamRespository.GetAll().Where(x => x.IsDeleted == false).Include(x => x.Branch);
                var list = query.Select(m => new TeamDto
                {
                    Id = m.Id,
                    name = m.name,
                    NameBranch = m.Branch.Name,
                    BranchId = m.Id,
                }).ToList();
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



        public AppResponse<TeamDto> GetTeamId(Guid Id)
        {
            var result = new AppResponse<TeamDto>();
            try
            {
                var tuyendung = _teamRespository.Get(Id);
                var data = _mapper.Map<TeamDto>(tuyendung);
                result.IsSuccess = true;
                result.Data = data;
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
                return result;

            }
        }

		public AppResponse<SearchResponse<TeamDto>> Search(SearchRequest request)
		{
			var result = new AppResponse<SearchResponse<TeamDto>>();
			try
			{
				var query = BuildFilterExpression(request.Filters);
				var numOfRecords = _teamRespository.CountRecordsByPredicate(query);
				var model = _teamRespository.FindByPredicate(query).Include(x=>x.Branch);
                if (request.SortBy != null)
                {
                    model = AddSort(model, request.SortBy);
                }
                int pageIndex = request.PageIndex ?? 1;
				int pageSize = request.PageSize ?? 1;
				int startIndex = (pageIndex - 1) * (int)pageSize;
				var List = model.Skip(startIndex).Take(pageSize)
					.Select(x => new TeamDto
					{
						Id = x.Id,
						name = x.name,
                        BranchId = x.BranchId,
                        NameBranch = x.Branch.Name
					})
					.ToList();


				var searchUserResult = new SearchResponse<TeamDto>
				{
					TotalRows = numOfRecords,
					TotalPages = CalculateNumOfPages(numOfRecords, pageSize),
					CurrentPage = pageIndex,
					Data = List,
				};
				result.BuildResult(searchUserResult);
			}
			catch (Exception ex)
			{
				result.BuildError(ex.Message);
			}
			return result;
		}

        private IQueryable<Team> AddSort(IQueryable<Team> input, SortByInfo sortByInfo)
        {
            var result = input.AsQueryable();
            switch (sortByInfo.FieldName)
            {

                case "nameBranch":
                    {
                        if (sortByInfo.Ascending != null && sortByInfo.Ascending.Value)
                        {
                            result = result.OrderBy(m => m.Branch.Name);

                        }
                        else
                        {
                            result = result.OrderByDescending(m => m.Branch.Name);
                        }
                    }
                    break;
                case "name":
                    {
                        if (sortByInfo.Ascending != null && sortByInfo.Ascending.Value)
                        {
                            result = result.OrderBy(m => m.name);

                        }
                        else
                        {
                            result = result.OrderByDescending(m => m.name);
                        }
                    }
                    break;
                default:
                    break;
            }
            return result;
        }

        private ExpressionStarter<Team> BuildFilterExpression(IList<Filter> Filters)
		{
			try
			{
				var predicate = PredicateBuilder.New<Team>(true);
				if (Filters != null)
					foreach (var filter in Filters)
					{
						switch (filter.FieldName)
						{
							case "name":
								predicate = predicate.And(m => m.name.Contains(filter.Value));
								break;
							//case "IsDelete":
							//	{
							//		bool isDetete = false;
							//		if (filter.Value == "True" || filter.Value == "true")
							//		{
							//			isDetete = true;
							//		}
							//		predicate = predicate.And(m => m.IsDeleted == isDetete);
							//	}
							//	break;
							default:
								break;
						}
					}
				predicate = predicate.And(m => m.IsDeleted == false);
				return predicate;
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}
