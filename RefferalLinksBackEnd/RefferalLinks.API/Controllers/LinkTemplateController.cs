using MayNghien.Models.Request.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RefferalLinks.Models.Dto;
using RefferalLinks.Service.Contract;
using RefferalLinks.Service.Implementation;

namespace RefferalLinks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class LinkTemplateController : Controller
    {
        private ILinkTemplateService _templateService;
        public LinkTemplateController(ILinkTemplateService templateService)
        {
            _templateService = templateService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _templateService.GetAll();
            return Ok(result);
        }
        [HttpGet]
        [Route("{Id}")]
        public IActionResult Get(Guid Id)
        {
            var reuslt = _templateService.Get(Id);
            return Ok(reuslt);
        }
        [HttpPost]
        public IActionResult Create(LinkTemplateDto request)
        {
            var result = _templateService.Create(request);
            return Ok(result);
        }
        [HttpDelete]
        [Route("{Id}")]
        public IActionResult Delete(Guid Id)
        {
            var result = _templateService.Delete(Id);
            return Ok(result);
        }
        [HttpPut]
        //[Route("{Id}")]
        public IActionResult Edit(LinkTemplateDto request)
        {
            var reuslt = _templateService.Edit(request);
            return Ok(reuslt);
        }
        [HttpPost]
        [Route("search")]
        public IActionResult Search(SearchRequest request)
        {
            var result = _templateService.Search(request);
            return Ok(result);
        }
        [HttpPut]
        [Route("{Id}")]
        public IActionResult StatusChange(LinkTemplateDto Id)
        {
            var reuslt = _templateService.StatusChange(Id);
            return Ok(reuslt);
        }
    }
}
