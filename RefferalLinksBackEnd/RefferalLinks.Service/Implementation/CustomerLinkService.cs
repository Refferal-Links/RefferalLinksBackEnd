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
      
        public CustomerLinkService(ICustomerLinkRepository customerLinkRepository, IMapper mapper , ICustomerRespository customerRespository , ILinkTemplateRepository linkTemplateRepository)
        {
            _customerLinkRepository = customerLinkRepository;
            _mapper = mapper;
            _customerRespository = customerRespository;
            _linkTemplateRepository = linkTemplateRepository;
        }

        public AppResponse<CustomerLinkDto> Create(CustomerLinkDto request)
        {
            var result = new AppResponse<CustomerLinkDto>();
            try
            {
              var linktemplatelist = _linkTemplateRepository.GetAll();
              foreach(var linktemplate in linktemplatelist)
                {                
                    var customerlink = new Customerlink();
                    customerlink.Id = Guid.NewGuid();
                    customerlink.LinkTemplateId = linktemplate.Id;
                    customerlink.CustomerId = request.CustomerId;
                    customerlink.Url = linktemplate.Url;
                    customerlink.Url = linktemplate.Url.Replace("{{sale}}", customerlink.Customer.ApplicationUser.RefferalCode);
                    customerlink.Url = linktemplate.Url.Replace("{{ten}}" , customerlink.Customer.Name );
                    customerlink.Url = linktemplate.Url.Replace("{{phone}}", customerlink.Customer.PhoneNumber );
                    customerlink.Url = linktemplate.Url.Replace("{{email}}", customerlink.Customer.Email );
                    customerlink.Url = linktemplate.Url.Replace("{{Province}}", customerlink.Customer.Province.Name);                 
                    request.CustomerId = customerlink.CustomerId;
                    request.Id = customerlink.Id;
                    request.LinkTemplateId = customerlink.LinkTemplateId;
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
