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
    public interface ICustomerService
    {
        AppResponse<List<CustomerDto>> GetAll();
        AppResponse<CustomerDto> Get(Guid Id);
        AppResponse<CustomerDto> Edit(CustomerDto request);
        AppResponse<string> Delete(Guid Id);
        AppResponse<CustomerDto> Create(CustomerDto request);
        AppResponse<SearchResponse<CustomerDto>> Search(SearchRequest request);
    }
}
