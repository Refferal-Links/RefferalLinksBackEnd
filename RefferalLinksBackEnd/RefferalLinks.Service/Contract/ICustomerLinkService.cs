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
        AppResponse<CustomerLinkDto> Create(CustomerLinkDto request);
    }
}
