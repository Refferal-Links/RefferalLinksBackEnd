using MayNghien.Models.Request.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RefferalLinks.Models.Dto;
using RefferalLinks.Service.Contract;

namespace RefferalLinks.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CustomerLinktroller : Controller
	{
		private ICustomerLinkService _customerLinkService;
		public CustomerLinktroller(ICustomerLinkService customerLinkService)
		{
			_customerLinkService= customerLinkService;
		}

		
		[HttpPost]
		public IActionResult Create(CustomerLinkDto request)
		{
			var result = _customerLinkService.Create(request);
			return Ok(result);
		}
	
	}
}
