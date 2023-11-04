using System.Linq.Expressions;
using RefferalLinks.DAL.Contract;
using RefferalLinks.DAL.Models.Context;
using RefferalLinks.DAL.Models.Entity;

namespace RefferalLinks.DAL.Implementation
{
	public class UserRespository : IUserespository
    {
        private readonly RefferalLinksDbContext _context;
        public UserRespository( RefferalLinksDbContext context)
        {
            _context = context;
        }
        public List<ApplicationUser> GetAll()
        {
            return _context.Users.ToList();
        }

        public int CountRecordsByPredicate(Expression<Func<ApplicationUser, bool>> predicate)
        {
            return _context.Users.Where(predicate).Count();
        }

        public ApplicationUser FindById(string id)
        {
            return _context.Users.Where(m => m.Id == id).FirstOrDefault();
        }

        public IQueryable<ApplicationUser> FindByPredicate(Expression<Func<ApplicationUser, bool>> predicate)
        {
            return _context.Users.Where(predicate).AsQueryable();
        }

        public ApplicationUser FindUser(string? Id)
        {
            return _context.Users.FirstOrDefault(m => m.Id == Id);
        }
        public ApplicationUser FindByEmail(string? email)
        {

			ApplicationUser user = _context.Users.Where(n => n.Email == email).FirstOrDefault();

            return user;
        }
    }
}
