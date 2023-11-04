using Maynghien.Common.Repository;
using Microsoft.AspNetCore.Identity;
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
    public class TeamRespository : GenericRepository<TeamManagement, RefferalLinksDbContext, ApplicationUser>, ITeamRespository
    {
        public TeamRespository(RefferalLinksDbContext unitOfWork) : base(unitOfWork)
        {
            _context = unitOfWork;
        }
        public int CountRecordsByPredicate(Expression<Func<TeamManagement, bool>> predicate)
        {
            return _context.TeamManagement.Where(predicate).Count();
        }
        public IQueryable<TeamManagement> FindByPredicate(Expression<Func<TeamManagement, bool>> predicate)
        {
            return _context.TeamManagement.Where(predicate).AsQueryable();
        }
    }
}
