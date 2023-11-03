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
	public interface ITeamRespository : IGenericRepository<TeamManagement, RefferalLinksDbContext>
    {
        public int CountRecordsByPredicate(Expression<Func<TeamManagement, bool>> predicate);
        public IQueryable<TeamManagement> FindByPredicate(Expression<Func<TeamManagement, bool>> predicate);
    }
}
