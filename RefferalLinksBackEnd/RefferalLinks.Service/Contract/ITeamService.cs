using MayNghien.Models.Request.Base;
using MayNghien.Models.Response.Base;
using RefferalLinks.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefferalLinks.Service.Contract
{
   public interface ITeamService
    {
        AppResponse<List<TeamDto>> GetAllTeam();
        AppResponse<TeamDto> GetTeamId(Guid Id);
        AppResponse<TeamDto> CreateTeam( TeamDto request);
        AppResponse<TeamDto> EditTeam(TeamDto request);
        AppResponse<string> DeleteTeam(Guid Id);
       
    }
}
