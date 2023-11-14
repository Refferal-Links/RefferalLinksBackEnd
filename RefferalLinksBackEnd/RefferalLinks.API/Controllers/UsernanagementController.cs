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
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class UserManagemetController : Controller
    {
        IUsermanagementService _usermanagementService;
        public UserManagemetController(IUsermanagementService usermanagementService)
        {
            _usermanagementService = usermanagementService;
        }
        [HttpGet]
        //[Authorize(Roles = "superadmin")]
        public  async Task< IActionResult> GetAll()
        {
            var result = await _usermanagementService.GetAllUser();
            return Ok(result);
        }
        [HttpPut]
        [Route("{Id}")]
        public async Task<IActionResult> RestPassWordUser(string Id)
        {
            var result = await _usermanagementService.ResetPassWordUser(Id);

            return Ok(result);
        }
        [HttpPost]
        //[Authorize(Roles = "superadmin")]
        public async Task<IActionResult> CreateUser([FromBody] UserModel request)
        {
            var result = await _usermanagementService.CreateUser(request);

            return Ok(result);
        }
        [HttpDelete]
        [Route("{Id}")]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            var result = await _usermanagementService.DeleteUser(Id);

            return Ok(result);
        }
       
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUserIdentity(string id)
        {
            var result = await _usermanagementService.GetUser(id);

            return Ok(result);
        }
		[HttpPost]
		[Route("search")]
		public async Task<IActionResult> Search(SearchRequest request)
		{
			var result = await _usermanagementService.Search(request);
			return Ok(result);
		}
        [HttpPut]
        [Route("StatusChange")]
        public async Task < IActionResult> Statuschange(string id)
        {
            var result = await _usermanagementService.StatusChange(id);
            return Ok(result);
        }
	}
}
