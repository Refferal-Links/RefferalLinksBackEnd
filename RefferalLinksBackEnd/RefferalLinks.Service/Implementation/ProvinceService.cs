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
	public class ProvinceService:IProvinceService
	{
		private readonly IProvinceRepository _provinceRepository;
		private IMapper _mapper;
		public ProvinceService(IProvinceRepository provinceRepository, IMapper mapper)
		{
			_provinceRepository = provinceRepository;
			_mapper = mapper;
		}

		public AppResponse<ProvinceDto> Create(ProvinceDto request)
		{
			var result = new AppResponse<ProvinceDto>();
			try
			{
				var Province = _mapper.Map<Province>(request);
				Province.Id = Guid.NewGuid();

				_provinceRepository.Add(Province);
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
				var province = _provinceRepository.Get(Id);
				province.IsDeleted = true;
				_provinceRepository.Edit(province);
				result.BuildResult("xóa thành công");
			}
			catch (Exception ex)
			{
				result.BuildError(ex.Message);
			}
			return result;
		}

		public AppResponse<ProvinceDto> Edit(ProvinceDto request)
		{
			var result = new AppResponse<ProvinceDto>();
			try
			{
				var province = _provinceRepository.Get((Guid)request.Id);
				province.Name = request.Name;
				_provinceRepository.Edit(province);
				result.BuildResult(request);
			}
			catch (Exception ex)
			{
				result.BuildError(ex.Message);
			}
			return result;
		}

		public AppResponse<ProvinceDto> Get(Guid Id)
		{
			var result = new AppResponse<ProvinceDto>();
			try
			{
				var province = _provinceRepository.Get(Id);
				var data = _mapper.Map<ProvinceDto>(province);
				result.BuildResult(data);
			}
			catch (Exception ex)
			{
				result.BuildError(ex.Message);
			}
			return result;
		}

		public AppResponse<List<ProvinceDto>> GetAll()
		{
			var result = new AppResponse<List<ProvinceDto>>();
			try
			{
				var list = _provinceRepository.GetAll().OrderBy(x => x.Name).Where(x=>x.IsDeleted != true).Select(x => new ProvinceDto
				{
					Id = x.Id,
					Name = x.Name,
				}).ToList();
				result.BuildResult(list);
			}
			catch (Exception ex)
			{
				result.BuildError(ex.Message);
			}
			return result;
		}

		public AppResponse<SearchResponse<ProvinceDto>> Search(SearchRequest request)
		{
			var result = new AppResponse<SearchResponse<ProvinceDto>>();
			try
			{
				var query = BuildFilterExpression(request.Filters);
				var numOfRecords = _provinceRepository.CountRecordsByPredicate(query);
				var model = _provinceRepository.FindByPredicate(query);
                if (request.SortBy != null)
                {
                    model = AddSort(model, request.SortBy);
                }
				else
				{
					model = model.OrderBy(x => x.Name);
				}
                int pageIndex = request.PageIndex ?? 1;
				int pageSize = request.PageSize ?? 1;
				int startIndex = (pageIndex - 1) * (int)pageSize;
				var List = model.Skip(startIndex).Take(pageSize)
					.Select(x => new ProvinceDto
					{
						Id = x.Id,
						Name = x.Name
					})
					.ToList();


				var searchUserResult = new SearchResponse<ProvinceDto>
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

        private IQueryable<Province> AddSort(IQueryable<Province> input, SortByInfo sortByInfo)
        {
            var result = input.AsQueryable();
            switch (sortByInfo.FieldName)
            {
                case "name":
                    {
                        if (sortByInfo.Ascending != null && sortByInfo.Ascending.Value)
                        {
                            result = result.OrderBy(m => m.Name);

                        }
                        else
                        {
                            result = result.OrderByDescending(m => m.Name);
                        }
                    }
                    break;
                default:
                    break;
            }
            return result;
        }

        private ExpressionStarter<Province> BuildFilterExpression(IList<Filter> Filters)
		{
			try
			{
				var predicate = PredicateBuilder.New<Province>(true);
				if (Filters != null)
					foreach (var filter in Filters)
					{
						switch (filter.FieldName)
						{
							case "name":
								predicate = predicate.And(m => m.Name.Contains(filter.Value));
								break;
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
