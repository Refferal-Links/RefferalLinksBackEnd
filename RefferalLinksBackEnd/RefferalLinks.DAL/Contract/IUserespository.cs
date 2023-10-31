using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RefferalLinks.DAL.Contract
{
    public interface IUserespository
    {
        public int CountRecordsByPredicate(Expression<Func<IdentityUser, bool>> predicate);
        public IQueryable<IdentityUser> FindByPredicate(Expression<Func<IdentityUser, bool>> predicate);
        public IdentityUser FindById(string id);
        public IdentityUser FindUser(string Id);
        public List<IdentityUser> GetAll();
        public IdentityUser FindByEmail(string? email);
    }
}
