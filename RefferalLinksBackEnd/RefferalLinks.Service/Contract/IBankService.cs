using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Models.Request.Base;
using MayNghien.Models.Response.Base;
using RefferalLinks.Models.Dto;

namespace RefferalLinks.Service.Contract
{
	public interface IBankService
	{
		AppResponse<List<BankDto>> GetAllBank();
		AppResponse<BankDto> GetBank(Guid Id);
		AppResponse<BankDto> CreateBank(BankDto request);
		AppResponse<BankDto> EditBank(BankDto request);
		AppResponse<string> DeleteBank(Guid Id);
		AppResponse<SearchResponse<BankDto>> Search(SearchRequest request);
	}
}
