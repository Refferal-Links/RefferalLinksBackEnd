using MayNghien.Models.Request.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RefferalLinks.Models.Dto;
using RefferalLinks.Service.Contract;
using RefferalLinks.Service.Implementation;

namespace RefferalLinks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,superadmin")]
    public class TeamController : Controller
    {
        private readonly ITeamService _TeamService;
        public TeamController(ITeamService TeamService)
        {
            _TeamService = TeamService;
        }
        [HttpGet]
        [Authorize(Roles = "superadmin")]
        public IActionResult GetAllTeam()
        {
            var result = _TeamService.GetAllTeam();
            return Ok(result);
        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetTeam(Guid id)
        {
            var result = _TeamService.GetTeamId(id);
            return Ok(result);
        }
        [HttpPost]
        public IActionResult CreateTeam(TeamDto request)
        {
            var result = _TeamService.CreateTeam(request);
            return Ok(result);
        }
        [HttpPut]
        public IActionResult EditTeam(TeamDto request)
        {
            var result = _TeamService.EditTeam(request);
            return Ok(result);
        }
        [HttpDelete]
		[Route("{Id}")]
		public IActionResult DeleteTeam(Guid Id)
        {

            var result = _TeamService.DeleteTeam(Id);

            return Ok(result);

        }
		[HttpPost]
		[Route("search")]
		public IActionResult Search(SearchRequest request)
		{
			var result = _TeamService.Search(request);
			return Ok(result);
		}
	}
}
