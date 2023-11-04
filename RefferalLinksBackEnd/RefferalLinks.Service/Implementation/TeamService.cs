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

        public TeamService(ITeamRespository teamRespository , IMapper mapper , IHttpContextAccessor httpContextAccessor) {
            _teamRespository = teamRespository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
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
                var team = new TeamManagement();
                team = _mapper.Map<TeamManagement>(request);
                team.Id = Guid.NewGuid();
                team.CreatedBy = UserName;

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
                var team = new TeamManagement();
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



        public AppResponse<TeamDto> EditTeam(TeamDto tuyendung)
        {
            var result = new AppResponse<TeamDto>();
            try
            {
                var UserName = ClaimHelper.GetClainByName(_httpContextAccessor, "UserName");
                var request = new TeamManagement();
                request = _mapper.Map<TeamManagement>(tuyendung);
                request.CreatedBy = UserName;
                _teamRespository.Edit(request);

                result.IsSuccess = true;
                result.Data = tuyendung;
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
                var query = _teamRespository.GetAll();
                var list = query.Select(m => new TeamDto
                {
                    Id = m.Id,
                   name = m.name,
                   RefferalCode = m.RefferalCode,

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
				var model = _teamRespository.FindByPredicate(query).OrderByDescending(x => x.CreatedOn);
				int pageIndex = request.PageIndex ?? 1;
				int pageSize = request.PageSize ?? 1;
				int startIndex = (pageIndex - 1) * (int)pageSize;
				var List = model.Skip(startIndex).Take(pageSize)
					.Select(x => new TeamDto
					{
						Id = x.Id,
						name = x.name,
                        RefferalCode = x.RefferalCode
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
		private ExpressionStarter<TeamManagement> BuildFilterExpression(IList<Filter> Filters)
		{
			try
			{
				var predicate = PredicateBuilder.New<TeamManagement>(true);
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
