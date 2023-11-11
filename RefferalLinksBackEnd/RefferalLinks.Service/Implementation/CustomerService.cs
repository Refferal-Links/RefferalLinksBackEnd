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
        private ILinkTemplateRepository _linkTemplateRepository;
        private IUserespository _userespository;
        private ICustomerLinkRepository _customerLinkRepository;
        private IBankRepository _bankRepository;
        public CustomerService(ICustomerRespository customerRespository, IMapper mapper,
            ILinkTemplateRepository linkTemplateRepository, IUserespository userespository, ICustomerLinkRepository customerLinkRepository, IBankRepository bankRepository)
        {
            _customerRespository = customerRespository;
            _mapper = mapper;
            _linkTemplateRepository = linkTemplateRepository;
            _userespository = userespository;
            _customerLinkRepository = customerLinkRepository;
            _bankRepository = bankRepository;
        }

        public AppResponse<CustomerDto> Create(CustomerDto request)
        {
            var result = new AppResponse<CustomerDto>();
            try
            {
                if (request.RefferalCode == null)
                {
                    return result.BuildError("Không để trống mã giớ thiệu");
                }
                var user = _userespository.FindByPredicate(x => x.RefferalCode == request.RefferalCode).FirstOrDefault(x => x.RefferalCode == request.RefferalCode);
                if (user == null)
                {
                    return result.BuildError("không tìm thấy mã giới thiệu");
                }
                var customer = _mapper.Map<Customer>(request);
                customer.Id = Guid.NewGuid();
                customer.ApplicationUserId = user.Id;
                _customerRespository.Add(customer);
                request.Banks = _bankRepository.GetAll().Select(x => new BankDto
                {
                    Name = x.Name,
                    Id = x.Id,
                    CustomerLinks = new List<CustomerLinkDto>()
                }).ToList();
                var linktemplatelist = _linkTemplateRepository.GetAll().Where(x => x.IsActive == true && x.IsDeleted == false).Include(x => x.Bank).ToList(); ;
                foreach (var linktemplate in linktemplatelist)
                {
                    var customerlink = new Customerlink();
                    customerlink.Id = Guid.NewGuid();
                    customerlink.LinkTemplateId = linktemplate.Id;
                    customerlink.CustomerId = customer.Id;
                    customerlink.Url = linktemplate.Url;
                    customerlink.Url = customerlink.Url.Replace("{{sale}}", request.RefferalCode);
                    customerlink.Url = customerlink.Url.Replace("{{ten}}", customer.Name);
                    customerlink.Url = customerlink.Url.Replace("{{phone}}", customer.PhoneNumber);
                    customerlink.Url = customerlink.Url.Replace("{{cccd}}", customer.Cccd);
                    customerlink.Url = customerlink.Url.Replace("{{email}}", customer.Email);
                    _customerLinkRepository.Add(customerlink);

                    var data = _mapper.Map<CustomerLinkDto>(customerlink);
                    request.Banks.FirstOrDefault(x => x.Id == linktemplate.BankId).CustomerLinks.Add(data);
                }

                request.Id = Guid.NewGuid();
                result.BuildResult(request);
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }

        //public AppResponse<CustomerDto> Create(CustomerDto request)
        //{
        //    var result = new AppResponse<CustomerDto>();
        //    try
        //    {
        //        if (request.RefferalCode == null)
        //        {
        //            return result.BuildError("Không để trống mã giớ thiệu");
        //        }
        //        var user = _userespository.FindByPredicate(x => x.RefferalCode == request.RefferalCode).FirstOrDefault(x => x.RefferalCode == request.RefferalCode);
        //        if (user == null)
        //        {
        //            return result.BuildError("không tìm thấy mã giới thiệu");
        //        }
        //        var customer = _mapper.Map<Customer>(request);
        //        customer.Id = Guid.NewGuid();
        //        customer.ApplicationUserId = user.Id;
        //        _customerRespository.Add(customer);
        //        request.Banks = new List<BankDto>();
        //        var listcustomedlink = new List<CustomerLinkDto>();
        //        var addlistbank = new BankDto();
        //        var listbank = _bankRepository.GetAll().Where(x => x.IsDeleted == false).ToList();
        //        foreach(var bank in listbank)
        //        {

        //            var linktemplatelist = _linkTemplateRepository.GetAll().Where(x => x.IsActive == true && x.IsDeleted == false).ToList();

        //            addlistbank.Name = bank.Name;
        //            foreach (var linktemplate in linktemplatelist)
        //            {
        //                if(bank.Id == linktemplate.BankId)
        //                {
        //                var customerlink = new Customerlink();
        //                customerlink.Id = Guid.NewGuid();
        //                customerlink.LinkTemplateId = linktemplate.Id;
        //                customerlink.CustomerId = customer.Id;
        //                customerlink.Url = linktemplate.Url;
        //                customerlink.Url = customerlink.Url.Replace("{{sale}}", request.RefferalCode);
        //                customerlink.Url = customerlink.Url.Replace("{{ten}}", customer.Name);
        //                customerlink.Url = customerlink.Url.Replace("{{phone}}", customer.PhoneNumber);
        //                customerlink.Url = customerlink.Url.Replace("{{cccd}}", customer.Cccd);
        //                customerlink.Url = customerlink.Url.Replace("{{email}}", customer.Email);
        //                  _customerLinkRepository.Add(customerlink);
        //                  var data = _mapper.Map<CustomerLinkDto>(customerlink);
        //                    if (addlistbank.CustomerLinks == null)
        //                    {
        //                        addlistbank.CustomerLinks = new List<CustomerLinkDto>();
        //                    }
        //                    addlistbank.CustomerLinks.Add(data);                   

        //                }
        //                else
        //                {
        //                    continue;
        //                }
        //                //var data = _mapper.Map<CustomerLinkDto>(customerlink);
        //                //_customerLinkRepository.Add(customerlink);

        //            } 
        //            request.Banks.Add(addlistbank);

        //        }

        //        request.Id = Guid.NewGuid();
        //        result.BuildResult(request);

        //    }
        //    catch (Exception ex)
        //    {
        //        result.BuildError(ex.Message);
        //    }
        //    return result;
        //}



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
                var user = _userespository.FindByPredicate(x => x.RefferalCode == request.RefferalCode).FirstOrDefault(x => x.RefferalCode == request.RefferalCode);
                var customer = _customerRespository.Get((Guid)request.Id);
                customer.Name = request.Name;
                customer.Email = request.Email;
                customer.Passport = request.Passport;
                customer.ApplicationUserId = user.Id;
                customer.PhoneNumber = request.PhoneNumber;
                customer.ProvinceId = request.ProvinceId;
                _customerRespository.Edit(customer);
                var listCustomerLink = _customerLinkRepository.FindBy(x=>x.CustomerId == customer.Id).ToList();
                _customerLinkRepository.DeleteRange(listCustomerLink);
                var linktemplatelist = _linkTemplateRepository.GetAll().Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                foreach (var linktemplate in linktemplatelist)
                {
                    var customerlink = new Customerlink();
                    customerlink.Id = Guid.NewGuid();
                    customerlink.LinkTemplateId = linktemplate.Id;
                    customerlink.CustomerId = customer.Id;
                    customerlink.Url = linktemplate.Url;
                    var code = _userespository.FindUser(customer.ApplicationUserId).RefferalCode;
                    customerlink.Url = customerlink.Url.Replace("{{sale}}", code);
                    customerlink.Url = customerlink.Url.Replace("{{ten}}", customer.Name);
                    customerlink.Url = customerlink.Url.Replace("{{phone}}", customer.PhoneNumber);
                    customerlink.Url = customerlink.Url.Replace("{{cccd}}", customer.Cccd);
                    customerlink.Url = customerlink.Url.Replace("{{email}}", customer.Email);
                    _customerLinkRepository.Add(customerlink);
                    //request.CustomerLinks.Add(_mapper.Map<CustomerLinkDto>(customerlink));
                }

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
                var query = _customerRespository.FindBy(x => x.Id == Id).Include(x => x.Province).Include(x => x.ApplicationUser) ;

                var data = query.Select(x => new CustomerDto
                {
                   Id = x.Id,
                   Email = x.Email,
                   Passport = x.Passport,
                   ProvinceId = x.ProvinceId,
                   PhoneNumber = x.PhoneNumber,
                   Name = x.Name,
                   Cccd = x.Cccd,
                   RefferalCode = x.ApplicationUser.RefferalCode,
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
                var Customeres  = _customerRespository.GetAll().Include(x => x.Province).ToList();
                var list = Customeres.Select(x => {
                    var code = _userespository.FindByPredicate(m => m.Id == x.ApplicationUserId).FirstOrDefault().RefferalCode;
                    var customerDto = new CustomerDto();
                    customerDto = _mapper.Map<CustomerDto>(x);
                    //customerDto.CustomerLinks = new List<CustomerLinkDto>();
                    var listCustomerLink = _customerLinkRepository.GetAll().Where(x=>x.CustomerId == (Guid)customerDto.Id).ToList();
                   /* customerDto.CustomerLinks.AddRange(_mapper.Map<List<CustomerLinkDto>>(listCustomerLink))*/;
                    return customerDto;
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
