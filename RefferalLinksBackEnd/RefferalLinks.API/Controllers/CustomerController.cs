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
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CustomerController : Controller
	{
		private ICustomerService _customerService;
		public CustomerController(ICustomerService customerService)
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
		[HttpPost]
		public IActionResult Create(CustomerDto request)
		{
			var result = _customerService.Create(request);
			return Ok(result);
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
		public IActionResult Edit(CustomerDto request)
		{
			var reuslt = _customerService.Edit(request);
			return Ok(reuslt);
		}
		[HttpPost]
		[Route("search")]
		public IActionResult Search(SearchRequest request)
		{
			var result =_customerService.Search(request);
			return Ok(result);
		}
	}
}
