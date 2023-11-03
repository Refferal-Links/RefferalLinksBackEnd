using System.Linq.Expressions;
using Maynghien.Common.Models;

namespace RefferalLinks.DAL.Contract
{
	public interface IUserespository
    {
        public int CountRecordsByPredicate(Expression<Func<AspNetUser, bool>> predicate);
        public IQueryable<AspNetUser> FindByPredicate(Expression<Func<AspNetUser, bool>> predicate);
        public AspNetUser FindById(string id);
        public AspNetUser FindUser(string Id);
        public List<AspNetUser> GetAll();
        public AspNetUser FindByEmail(string? email);
    }
}
