using System.Linq.Expressions;
using RefferalLinks.DAL.Models.Entity;

namespace RefferalLinks.DAL.Contract
{
	public interface IUserespository
    {
        public int CountRecordsByPredicate(Expression<Func<ApplicationUser, bool>> predicate);
        public IQueryable<ApplicationUser> FindByPredicate(Expression<Func<ApplicationUser, bool>> predicate);
        public ApplicationUser FindById(string id);
        public ApplicationUser FindUser(string Id);
        public List<ApplicationUser> GetAll();
        public ApplicationUser FindByEmail(string? email);

        public void Edit(ApplicationUser user);

        public void Delete(string Id);

        ApplicationUser UserWithCustomerCount();
        List<ApplicationUser> GetListTeamLeader();
    }
}
