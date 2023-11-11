using AutoMapper;
using LinqKit;
using MayNghien.Models.Request.Base;
using MayNghien.Models.Response.Base;
using RefferalLinks.DAL.Contract;
using RefferalLinks.DAL.Implementation;
using RefferalLinks.DAL.Models.Entity;
using RefferalLinks.Models.Dto;
using RefferalLinks.Service.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static Maynghien.Common.Helpers.SearchHelper;
namespace RefferalLinks.Service.Implementation
{
    public class CustomerLinkService : ICustomerLinkService
    {
        private readonly ICustomerLinkRepository _customerLinkRepository;
        private IMapper _mapper;
        private readonly ICustomerRespository _customerRespository;
        private readonly ILinkTemplateRepository _linkTemplateRepository;
        private readonly IUserespository _userespository;
      
        public CustomerLinkService(ICustomerLinkRepository customerLinkRepository, IMapper mapper , ICustomerRespository customerRespository ,
            ILinkTemplateRepository linkTemplateRepository, IUserespository userespository)
        {
            _customerLinkRepository = customerLinkRepository;
            _mapper = mapper;
            _customerRespository = customerRespository;
            _linkTemplateRepository = linkTemplateRepository;
            _userespository = userespository;
        }

        public AppResponse<List<CustomerLinkDto>> GetAll()
        {
            var result = new AppResponse<List<CustomerLinkDto>>();
            try
            {
                var list = _customerLinkRepository.GetAll().Select(x => new CustomerLinkDto
                {
                    Id = x.Id,
                    Url = x.Url,
                    CustomerId = x.CustomerId,
                    LinkTemplateId = x.LinkTemplateId,
                }).ToList();
                result.BuildResult(list);
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
                var customer = _customerLinkRepository.Get(Id);
                customer.IsDeleted = true;
                _customerLinkRepository.Edit(customer);
                result.BuildResult("xóa thành công");
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }

        public AppResponse<CustomerLinkDto> Edit(CustomerLinkDto request)
        {
            var result = new AppResponse<CustomerLinkDto>();
            try
            {
                var customer = _customerLinkRepository.Get((Guid)request.Id);
                customer.Url = request.Url;
                customer.CustomerId = request.CustomerId;
                customer.LinkTemplateId = request.LinkTemplateId;
                _customerLinkRepository.Edit(customer);
                result.BuildResult(request);
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }

        public AppResponse<CustomerLinkDto> Get(Guid Id)
        {
            var result = new AppResponse<CustomerLinkDto>();
            try
            {
                var query = _customerLinkRepository.FindBy(x => x.Id == Id).Include(x => x.LinkTemplate) .Include(x => x.Customer);

                var data = query.Select(x => new CustomerLinkDto
                {
                    Id = x.Id,
                    Url = x.Url,
                    CustomerId = x.CustomerId,
                    LinkTemplateId = x.LinkTemplateId,
                }).First();
                result.BuildResult(data);
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }

        public AppResponse<SearchResponse<CustomerLinkDto>> Search(SearchRequest request)
        {
            var result = new AppResponse<SearchResponse<CustomerLinkDto>>();
            try
            {
                var query = BuildFilterExpression(request.Filters);
                var numOfRecords = _customerLinkRepository.CountRecordsByPredicate(query);
                var model = _customerLinkRepository.FindByPredicate(query).OrderByDescending(p => p.CreatedOn);
                int pageIndex = request.PageIndex ?? 1;
                int pageSize = request.PageSize ?? 1;
                int startIndex = (pageIndex - 1) * (int)pageSize;
                var List = model.Skip(startIndex).Take(pageSize)
                    .Select(x => new CustomerLinkDto
                    {
                        Id = x.Id,
                        Url = x.Url,
                        CustomerId = x.CustomerId,
                        LinkTemplateId= x.LinkTemplateId,
                    })
                    .ToList();


                var searchUserResult = new SearchResponse<CustomerLinkDto>
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
        private ExpressionStarter<Customerlink> BuildFilterExpression(IList<Filter> Filters)
        {
            try
            {
                var predicate = PredicateBuilder.New<Customerlink>(true);
                if (Filters != null)
                    foreach (var filter in Filters)
                    {
                        switch (filter.FieldName)
                        {
                            case "URL":
                                predicate = predicate.And(m => m.Url.Contains(filter.Value));
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
