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
	public interface ICampaignService
	{
		AppResponse<List<CampaignDto>> GetAll();
		AppResponse<CampaignDto> Get(Guid Id);
		AppResponse<CampaignDto> Edit(CampaignDto request);
		AppResponse<string> Delete(Guid Id);
		AppResponse<CampaignDto> Create(CampaignDto request);
		AppResponse<SearchResponse<CampaignDto>> Search(SearchRequest request);
		public AppResponse<string> StatusChange(CampaignDto request);

    }
}
