using MayNghien.Models.Request.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RefferalLinks.Models.Dto;
using RefferalLinks.Service.Contract;

namespace RefferalLinks.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProvinceController : Controller
	{
		private IProvinceService _provinceService;
		public ProvinceController(IProvinceService provinceService)
		{
			_provinceService = provinceService;
		}

		[HttpGet]
		public IActionResult GetAll()
		{
			var result = _provinceService.GetAll();
			return Ok(result);
		}
		[HttpGet]
		[Route("{Id}")]
		public IActionResult Get(Guid Id)
		{
			var reuslt = _provinceService.Get(Id);
			return Ok(reuslt);
		}
		[HttpPost]
		public IActionResult Create(ProvinceDto request)
		{
			var result = _provinceService.Create(request);
			return Ok(result);
		}
		[HttpDelete]
		[Route("{Id}")]
		public IActionResult Delete(Guid Id)
		{
			var result = _provinceService.Delete(Id);
			return Ok(result);
		}
		[HttpPut]
		//[Route("{Id}")]
		public IActionResult Edit(ProvinceDto request)
		{
			var reuslt = _provinceService.Edit(request);
			return Ok(reuslt);
		}
		[HttpPost]
		[Route("search")]
		public IActionResult Search(SearchRequest request)
		{
			var result = _provinceService.Search(request);
			return Ok(result);
		}
	}
}
