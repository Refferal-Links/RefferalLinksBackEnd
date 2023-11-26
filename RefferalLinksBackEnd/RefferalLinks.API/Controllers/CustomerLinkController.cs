using MayNghien.Models.Request.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RefferalLinks.Models.Dto;
using RefferalLinks.Service.Contract;
using Microsoft.AspNetCore.Authorization;

namespace RefferalLinks.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(AuthenticationSchemes = "Bearer")]
	public class CustomerLinkController : Controller
	{
		private ICustomerLinkService _customerService;
		public CustomerLinkController(ICustomerLinkService customerService)
		{
            _customerService = customerService;
		}

		[HttpGet]
		public IActionResult GetAll()
		{
			var result = _customerService.GetAll();
			return Ok(result);
		}
		[HttpGet]
		[Route("{Id}")]
		public IActionResult Get(Guid Id)
		{
			var reuslt = _customerService.Get(Id);
			return Ok(reuslt);
		}
		
		[HttpDelete]
		[Route("{Id}")]
		public IActionResult Delete(Guid Id)
		{
			var result = _customerService.Delete(Id);
			return Ok(result);
		}
		[HttpPut]
		//[Route("{Id}")]
		public IActionResult Edit(CustomerLinkDto request)
		{
			var reuslt = _customerService.Edit(request);
			return Ok(reuslt);
		}
		[HttpPost]
        [Route("search")]
        public async Task<IActionResult> Search(SearchRequest request)
        {
            var result = await _customerService.Search(request);
            return Ok(result);
        }
		[HttpPut]
		[Route("StatusChange")]
		public IActionResult StatusChange(CustomerLinkDto request)
		{
			var result = _customerService.StatusChange(request);
			return Ok(result);
		}
    }
}
