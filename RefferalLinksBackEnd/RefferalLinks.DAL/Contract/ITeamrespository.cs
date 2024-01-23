using Maynghien.Common.Repository;
using RefferalLinks.DAL.Models.Context;
using RefferalLinks.DAL.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RefferalLinks.DAL.Contract
{
	public interface ITeamRespository : IGenericRepository<Team, RefferalLinksDbContext, ApplicationUser>
    {
        public int CountRecordsByPredicate(Expression<Func<Team, bool>> predicate);
        public IQueryable<Team> FindByPredicate(Expression<Func<Team, bool>> predicate);
        public string GetTeamById(Guid teamId);
        public Dictionary<Guid, string> GetAllTeamNames();
        public Dictionary<Guid, string> GetAllbranhName();
    }
}
