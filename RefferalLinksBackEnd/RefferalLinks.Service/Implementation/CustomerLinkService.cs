using AutoMapper;
using MayNghien.Models.Response.Base;
using RefferalLinks.DAL.Contract;
using RefferalLinks.DAL.Implementation;
using RefferalLinks.DAL.Models.Entity;
using RefferalLinks.Models.Dto;
using RefferalLinks.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

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

        public AppResponse<CustomerLinkDto> Create(CustomerLinkDto request)
        {
            var result = new AppResponse<CustomerLinkDto>();
            try
            {
                if (request.CustomerId == null)
                {
                    return result.BuildError("customerId cannot null");
                }
                var customer = _customerRespository.Get(request.CustomerId);
                if (customer == null)
                {
                    return result.BuildError("cannot find customer");
                }
                var linktemplatelist = _linkTemplateRepository.GetAll();
                foreach(var linktemplate in linktemplatelist)
                {                
                    var customerlink = new Customerlink();
                    customerlink.Id = Guid.NewGuid();
                    customerlink.LinkTemplateId = linktemplate.Id;
                    customerlink.CustomerId = request.CustomerId;
                    customerlink.Url = request.Url;
                    var code =  _userespository.FindUser(customer.ApplicationUserId).RefferalCode;
                    customerlink.Url = customerlink.Url.Replace("{{sale}}", code);
                    customerlink.Url = customerlink.Url.Replace("{{ten}}", customer.Name);
                    customerlink.Url = customerlink.Url.Replace("{{phone}}", customer.PhoneNumber);
                    customerlink.Url = customerlink.Url.Replace("{{cccd}}", customer.Cccd);
                    customerlink.Url = customerlink.Url.Replace("{{email}}", customer.Email);
                    _customerLinkRepository.Add(customerlink);
                }

                result.BuildResult(request);
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }


    }
}
