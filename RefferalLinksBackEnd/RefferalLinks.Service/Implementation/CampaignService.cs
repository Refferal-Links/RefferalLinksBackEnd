using AutoMapper;
using LinqKit;
using MayNghien.Models.Request.Base;
using MayNghien.Models.Response.Base;
using RefferalLinks.DAL.Contract;
using RefferalLinks.DAL.Models.Entity;
using RefferalLinks.Models.Dto;
using RefferalLinks.Service.Contract;
using static Maynghien.Common.Helpers.SearchHelper;

namespace RefferalLinks.Service.Implementation
{
	public class CampaignService : ICampaignService
	{
		private readonly ICampaignRepository _campaignRepository;
        private IMapper _mapper;
		public CampaignService(ICampaignRepository campaignRepository, IMapper mapper)
		{
			_campaignRepository = campaignRepository;
			_mapper = mapper;
		}

		public AppResponse<CampaignDto> Create(CampaignDto request)
		{
			var result = new AppResponse<CampaignDto>();
			try
			{
				var campaign = _mapper.Map<Campaign>(request);
				campaign.Id = Guid.NewGuid();
				campaign.IsActive = true;
				_campaignRepository.Add(campaign);
				request.Id = Guid.NewGuid();
				result.BuildResult(request);
			}
			catch (Exception ex)
			{
				result.BuildError(ex.Message);
			}
			return result;
		}

		public AppResponse<string> Delete(Guid Id)
		{
			var result = new AppResponse<string>();
			try
			{
				var campaign = _campaignRepository.Get(Id);
				campaign.IsDeleted = true;
				_campaignRepository.Edit(campaign);
				result.BuildResult("xóa thành công");
			}
			catch (Exception ex)
			{
				result.BuildError(ex.Message);
			}
			return result;
		}

		public AppResponse<CampaignDto> Edit(CampaignDto request)
		{
			var result = new AppResponse<CampaignDto>();
			try
			{
				var campaign = _campaignRepository.Get((Guid)request.Id);
				campaign.Name = request.Name;
				campaign.IsActive = request.IsActive;
				_campaignRepository.Edit(campaign);
				result.BuildResult(request);
			}
			catch (Exception ex)
			{
				result.BuildError(ex.Message);
			}
			return result;
		}

		public AppResponse<CampaignDto> Get(Guid Id)
		{
			var result = new AppResponse<CampaignDto>();
			try
			{
				var campaign = _campaignRepository.Get(Id);
				var data = _mapper.Map<CampaignDto>(campaign);
				result.BuildResult(data);
			}
			catch (Exception ex)
			{
				result.BuildError(ex.Message);
			}
			return result;
		}

		public AppResponse<List<CampaignDto>> GetAll()
		{
			var result = new AppResponse<List<CampaignDto>>();
			try
			{
				var list = _campaignRepository.GetAll().Select(x => new CampaignDto
				{
					Id = x.Id,
					Name = x.Name,
					IsActive = x.IsActive,
				}).ToList();
				result.BuildResult(list);
			}
			catch (Exception ex)
			{
				result.BuildError(ex.Message);
			}
			return result;
		}

		public AppResponse<SearchResponse<CampaignDto>> Search(SearchRequest request)
		{
			var result = new AppResponse<SearchResponse<CampaignDto>>();
			try
			{
				var query = BuildFilterExpression(request.Filters);
				var numOfRecords = _campaignRepository.CountRecordsByPredicate(query);
				var model = _campaignRepository.FindByPredicate(query).OrderByDescending(p => p.CreatedOn);
				int pageIndex = request.PageIndex ?? 1;
				int pageSize = request.PageSize ?? 1;
				int startIndex = (pageIndex - 1) * (int)pageSize;
				var List = model.Skip(startIndex).Take(pageSize)
					.Select(x => new CampaignDto
					{
						Id = x.Id,
						Name = x.Name,
						IsActive = x.IsActive,
					})
					.ToList();


				var searchUserResult = new SearchResponse<CampaignDto>
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
		private ExpressionStarter<Campaign> BuildFilterExpression(IList<Filter> Filters)
		{
			try
			{
				var predicate = PredicateBuilder.New<Campaign>(true);
				if (Filters != null)
					foreach (var filter in Filters)
					{
						switch (filter.FieldName)
						{
							case "name":
								predicate = predicate.And(m => m.Name.Contains(filter.Value));
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
        public AppResponse<string> StatusChange(Guid Id)
        {
            var result = new AppResponse<string>();
            try
            {
                var budget = _campaignRepository.Get(Id);
                budget.IsActive = !budget.IsActive;

                _campaignRepository.Edit(budget);
                if (budget.IsActive)
                    result.BuildResult("active");
                else
                    result.BuildResult("inactive");
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }
    }
}
