using MayNghien.Models.Request.Base;
using MayNghien.Models.Response.Base;
using RefferalLinks.Models.Dto;

namespace RefferalLinks.Service.Contract
{
	public interface IProvinceService
	{
		AppResponse<List<ProvinceDto>> GetAll();
		AppResponse<ProvinceDto> Get(Guid Id);
		AppResponse<ProvinceDto> Edit(ProvinceDto request);
		AppResponse<string> Delete(Guid Id);
		AppResponse<ProvinceDto> Create(ProvinceDto request);
		AppResponse<SearchResponse<ProvinceDto>> Search(SearchRequest request);
	}
}
