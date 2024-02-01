using Maynghien.Common.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RefferalLinks.DAL.Contract;
using RefferalLinks.DAL.Models.Context;
using RefferalLinks.DAL.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RefferalLinks.DAL.Implementation
{
    public class TeamRespository : GenericRepository<Team, RefferalLinksDbContext, ApplicationUser>, ITeamRespository
    {
        public TeamRespository(RefferalLinksDbContext unitOfWork) : base(unitOfWork)
        {
            _context = unitOfWork;
        }
        public int CountRecordsByPredicate(Expression<Func<Team, bool>> predicate)
        {
            return _context.Team.Where(predicate).Count();
        }
        public IQueryable<Team> FindByPredicate(Expression<Func<Team, bool>> predicate)
        {
            return _context.Team.Where(predicate).AsQueryable();
        }
        public string GetTeamById(Guid teamId)
        {
            var team = _context.Team.FirstOrDefault(t => t.Id == teamId);
            // Replace Teams and TeamId with your actual entity and property names

            return team?.name ?? string.Empty;
            // Replace Teams and TeamId with your actual entity and property names
        }
        public Dictionary<Guid, string> GetAllTeamNames()
        {
            var teamNames = _context.Team.ToDictionary(team => team.Id, team => team.name);
            // Replace Teams, TeamId, and TeamName with your actual entity and property names

            return teamNames;
        }
        public Dictionary<Guid, string> GetAllbranhName()
        {
            var branchNames = _context.Branch.ToDictionary(branch => branch.Id, team => team.Name);
            // Replace Teams, TeamId, and TeamName with your actual entity and property names

            return branchNames;
        }
    }
}
