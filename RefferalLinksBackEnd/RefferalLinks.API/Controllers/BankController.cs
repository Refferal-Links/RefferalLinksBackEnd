using MayNghien.Models.Request.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RefferalLinks.Models.Dto;
using RefferalLinks.Service.Contract;

namespace RefferalLinks.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BankController : Controller
	{
		private IBankService _bankService;
		public BankController(IBankService bankService)
		{
			_bankService = bankService;
		}

		[HttpGet]
		public IActionResult GetAll()
		{
			var result = _bankService.GetAllBank();
			return Ok(result);
		}
		[HttpGet]
		[Route("{Id}")]
		public IActionResult Get(Guid Id)
		{
			var reuslt = _bankService.GetBank(Id);
			return Ok(reuslt);
		}
		[HttpPost]
		public IActionResult Create(BankDto request)
		{
			var result = _bankService.CreateBank(request);
			return Ok(result);
		}
		[HttpDelete]
		[Route("{Id}")]
		public IActionResult Delete(Guid Id)
		{
			var result =_bankService.DeleteBank(Id);
			return Ok(result);
		}
		[HttpPut]
		//[Route("{Id}")]
		public IActionResult Edit(BankDto request)
		{
			var reuslt = _bankService.EditBank(request);
			return Ok(reuslt);
		}
		[HttpPost]
		[Route("search")]
		public IActionResult Search(SearchRequest request)
		{
			var result =_bankService.Search(request);
			return Ok(result);
		}
	}
}
