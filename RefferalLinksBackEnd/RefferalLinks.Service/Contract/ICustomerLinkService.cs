using MayNghien.Models.Request.Base;
using MayNghien.Models.Response.Base;
using RefferalLinks.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefferalLinks.Service.Contract
{
  public interface ICustomerLinkService
    {
        //AppResponse<CustomerLinkDto> Create(CustomerLinkDto request);
    public    AppResponse<CustomerLinkDto> Get(Guid Id);
      public  AppResponse<CustomerLinkDto> Edit(CustomerLinkDto request);
       public AppResponse<string> Delete(Guid Id);
       public  Task<AppResponse<SearchResponse<CustomerLinkDto>>> Search(SearchRequest request);
        public AppResponse<List<CustomerLinkDto>> GetAll();
    }
}
