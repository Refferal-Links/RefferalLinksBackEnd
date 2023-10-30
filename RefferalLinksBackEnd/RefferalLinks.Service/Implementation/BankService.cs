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
	public class BankService : IBankService
	{
		private readonly IBankRepository _bankRepository;
		private readonly IMapper _mapper;
		public BankService(IBankRepository bankRepository, IMapper mapper)
		{
			_bankRepository = bankRepository;
			_mapper = mapper;
		}

		public AppResponse<BankDto> CreateBank(BankDto request)
		{
			var result = new AppResponse<BankDto>();
			try
			{
				var bank = _mapper.Map<Bank>(request);
				bank.Id = Guid.NewGuid();
				_bankRepository.Add(bank);
				request.Id = bank.Id;
				result.BuildResult(request);
			}
			catch (Exception ex)
			{
				result.BuildError(ex.Message);
			}
			return result;
		}

		public AppResponse<string> DeleteBank(Guid Id)
		{
			var result = new AppResponse<string>();
			try
			{
				var bank = _bankRepository.Get(Id);
				bank.IsDeleted = true;
				_bankRepository.Edit(bank);
				result.BuildResult("xóa thành công");
			}
			catch (Exception ex)
			{
				result.BuildError(ex.Message);
			}
			return result;
		}

		public AppResponse<BankDto> EditBank(BankDto request)
		{
			var result = new AppResponse<BankDto>();
			try
			{
				var bank = _bankRepository.Get((Guid)request.Id);
				bank.Name = request.Name;
				_bankRepository.Edit(bank);
				result.BuildResult(request);
			}
			catch (Exception ex)
			{
				result.BuildError(ex.Message);
			}
			return result;
		}

		public AppResponse<List<BankDto>> GetAllBank()
		{
			var result = new AppResponse<List<BankDto>>();
			try
			{
				var list = _bankRepository.GetAll().Select(x => new BankDto
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

		public AppResponse<BankDto> GetBank(Guid Id)
		{
			var result = new AppResponse<BankDto>();
			try
			{
				var bank = _bankRepository.Get(Id);
				var data = _mapper.Map<BankDto>(bank);
				result.BuildResult(data);
			}
			catch (Exception ex)
			{
				result.BuildError(ex.Message);
			}
			return result;
		}

		public AppResponse<SearchResponse<BankDto>> Search(SearchRequest request)
		{
			var result = new AppResponse<SearchResponse<BankDto>>();
			try
			{
				var query = BuildFilterExpression(request.Filters);
				var numOfRecords = _bankRepository.CountRecordsByPredicate(query);
				var model = _bankRepository.FindByPredicate(query);
				int pageIndex = request.PageIndex ?? 1;
				int pageSize = request.PageSize ?? 1;
				int startIndex = (pageIndex - 1) * (int)pageSize;
				var List = model.Skip(startIndex).Take(pageSize)
					.Select(x => new BankDto
					{
						Id = x.Id,
						Name = x.Name
					})
					.ToList();


				var searchUserResult = new SearchResponse<BankDto>
				{
					TotalRows = 0,
					TotalPages = CalculateNumOfPages(0, pageSize),
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
		private ExpressionStarter<Bank> BuildFilterExpression(IList<Filter> Filters)
		{
			try
			{
				var predicate = PredicateBuilder.New<Bank>(true);
				if (Filters != null)
					foreach (var filter in Filters)
					{
						switch (filter.FieldName)
						{
							case "Name":
								predicate = predicate.And(m => m.Name.Contains(filter.Value));
								break;
							case "IsDelete":
								{
									bool isDetete = false;
									if (filter.Value == "True" || filter.Value == "true")
									{
										isDetete = true;
									}
									predicate = predicate.And(m => m.IsDeleted == isDetete);
								}
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
	}
}
