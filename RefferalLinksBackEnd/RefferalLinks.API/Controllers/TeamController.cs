﻿using MayNghien.Models.Request.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RefferalLinks.Models.Dto;
using RefferalLinks.Service.Contract;

namespace RefferalLinks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
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
        [Route("{Id}")]
        public IActionResult EditTeam(TeamDto request)
        {
            var result = _TeamService.EditTeam(request);
            return Ok(result);
        }
        [HttpDelete]
        public IActionResult DeleteTeam(Guid id)
        {

            var result = _TeamService.DeleteTeam(id);

            return Ok(result);

        }
       
    }
}
