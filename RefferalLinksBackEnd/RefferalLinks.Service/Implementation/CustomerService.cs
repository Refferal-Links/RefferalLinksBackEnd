using AutoMapper;
using LinqKit;
using MayNghien.Models.Request.Base;
using MayNghien.Models.Response.Base;
using RefferalLinks.DAL.Contract;
using RefferalLinks.DAL.Models.Entity;
using RefferalLinks.Models.Dto;
using RefferalLinks.Service.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Maynghien.Common.Helpers.SearchHelper;

namespace RefferalLinks.Service.Implementation
{
   public class CustomerService : ICustomerService
    {
        private readonly ICustomerRespository _customerRespository;
        private IMapper _mapper;
        public CustomerService(ICustomerRespository customerRespository, IMapper mapper)
        {
            _customerRespository = customerRespository;
            _mapper = mapper;
        }

        public AppResponse<CustomerDto> Create(CustomerDto request)
        {
            var result = new AppResponse<CustomerDto>();
            try
            {
                var customer = _mapper.Map<Customer>(request);
                customer.Id = Guid.NewGuid();
           
                _customerRespository.Add(customer);
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
                var customer = _customerRespository.Get(Id);
                customer.IsDeleted = true;
                _customerRespository.Edit(customer);
                result.BuildResult("xóa thành công");
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }

        public AppResponse<CustomerDto> Edit(CustomerDto request)
        {
            var result = new AppResponse<CustomerDto>();
            try
            {
                var customer = _customerRespository.Get((Guid)request.Id);
                customer.Name = request.Name;
                customer.Email = request.Email;
                customer.Passport = request.Passport;
                customer.ApplicationUserId = request.ApplicationUserId;
                customer.PhoneNumber = request.PhoneNumber;
                customer.ProvinceId = request.ProvinceId;
                _customerRespository.Edit(customer);
                result.BuildResult(request);
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }

        public AppResponse<CustomerDto> Get(Guid Id)
        {
            var result = new AppResponse<CustomerDto>();
            try
            {
                var query = _customerRespository.FindBy(x => x.Id == Id).Include(x => x.Province);
                var data = query.Select(x => new CustomerDto
                {
                   Id = x.Id,
                   Email = x.Email,
                   Passport = x.Passport,
                   ApplicationUserId = x.ApplicationUserId,
                   ProvinceId = x.ProvinceId,
                   PhoneNumber = x.PhoneNumber,
                   Name = x.Name,
                   NameProvice = x.Province.Name,
                }).First();
                result.BuildResult(data);
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }

        public AppResponse<List<CustomerDto>> GetAll()
        {
            var result = new AppResponse<List<CustomerDto>>();
            try
            {
                var list = _customerRespository.GetAll() .Include(x => x.Province) .Select(x => new CustomerDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Passport = x.Passport,
                    PhoneNumber = x.PhoneNumber,
                    Email = x.Email,
                    ProvinceId = x.ProvinceId,
                    NameProvice = x.Province.Name,
                    ApplicationUserId = x.ApplicationUserId,
                }).ToList();
                result.BuildResult(list);
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }

        public AppResponse<SearchResponse<CustomerDto>> Search(SearchRequest request)
        {
            var result = new AppResponse<SearchResponse<CustomerDto>>();
            try
            {
                var query = BuildFilterExpression(request.Filters);
                var numOfRecords = _customerRespository.CountRecordsByPredicate(query);
                var model = _customerRespository.FindByPredicate(query).OrderByDescending(p => p.CreatedOn);
                int pageIndex = request.PageIndex ?? 1;
                int pageSize = request.PageSize ?? 1;
                int startIndex = (pageIndex - 1) * (int)pageSize;
                var List = model.Skip(startIndex).Take(pageSize)
                    .Select(x => new CustomerDto
                    {
                        Id = x.Id,
                        Name = x.Name
                    })
                    .ToList();


                var searchUserResult = new SearchResponse<CustomerDto>
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
        private ExpressionStarter<Customer> BuildFilterExpression(IList<Filter> Filters)
        {
            try
            {
                var predicate = PredicateBuilder.New<Customer>(true);
                if (Filters != null)
                    foreach (var filter in Filters)
                    {
                        switch (filter.FieldName)
                        {
                            case "Name":
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
    }
}
