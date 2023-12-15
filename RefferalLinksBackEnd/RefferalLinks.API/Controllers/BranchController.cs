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
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, superadmin")]
    public class BranchController : Controller
    {
        private IBranchService _branchService;
        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll()
        {
            var result = _branchService.GetAll();
            return Ok(result);
        }
        [HttpGet]
        [Route("{Id}")]
        public IActionResult Get(Guid Id)
        {
            var reuslt = _branchService.Get(Id);
            return Ok(reuslt);
        }
        [HttpPost]
        public IActionResult Create(BranchDto request)
        {
            var result = _branchService.Create(request);
            return Ok(result);
        }
        [HttpDelete]
        [Route("{Id}")]
        public IActionResult Delete(Guid Id)
        {
            var result = _branchService.Delete(Id);
            return Ok(result);
        }
        [HttpPut]
        //[Route("{Id}")]
        public IActionResult Edit(BranchDto request)
        {
            var reuslt = _branchService.Edit(request);
            return Ok(reuslt);
        }
        [HttpPost]
        [Route("search")]
        public IActionResult Search(SearchRequest request)
        {
            var result = _branchService.Search(request);
            return Ok(result);
        }
    }
}
