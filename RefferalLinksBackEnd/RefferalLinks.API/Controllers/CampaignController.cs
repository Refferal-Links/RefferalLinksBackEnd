using MayNghien.Models.Request.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RefferalLinks.Models.Dto;
using RefferalLinks.Service.Contract;

namespace RefferalLinks.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, superadmin")]
    public class CampaignController : Controller
	{
		private ICampaignService _campaignService;
		public CampaignController(ICampaignService campaignService)
		{
			_campaignService = campaignService;
		}

		[HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll()
		{
			var result = _campaignService.GetAll();
			return Ok(result);
		}
		[HttpGet]
		[Route("{Id}")]
		public IActionResult Get(Guid Id)
		{
			var reuslt = _campaignService.Get(Id);
			return Ok(reuslt);
		}
		[HttpPost]
		public IActionResult Create(CampaignDto request)
		{
			var result = _campaignService.Create(request);
			return Ok(result);
		}
		[HttpDelete]
		[Route("{Id}")]
		public IActionResult Delete(Guid Id)
		{
			var result = _campaignService.Delete(Id);
			return Ok(result);
		}
		[HttpPut]
		//[Route("{Id}")]
		public IActionResult Edit(CampaignDto request)
		{
			var reuslt = _campaignService.Edit(request);
			return Ok(reuslt);
		}
		[HttpPost]
		[Route("search")]
		public IActionResult Search(SearchRequest request)
		{
			var result = _campaignService.Search(request);
			return Ok(result);
		}
		[HttpPut]
		[Route("StatusChange")]
		public IActionResult StatusChange([FromBody] CampaignDto request)
        {
            var result = _campaignService.StatusChange(request);
            return Ok(result);
        }
    }
}
