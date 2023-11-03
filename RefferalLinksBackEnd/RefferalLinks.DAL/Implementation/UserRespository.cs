using Maynghien.Common.Models;
using Microsoft.AspNetCore.Identity;
using RefferalLinks.DAL.Contract;
using RefferalLinks.DAL.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RefferalLinks.DAL.Implementation
{
    public class UserRespository : IUserespository
    {
        private readonly RefferalLinksDbContext _context;
        public UserRespository( RefferalLinksDbContext context)
        {
            _context = context;
        }
        public List<AspNetUser> GetAll()
        {
            return _context.Users.ToList();
        }

        public int CountRecordsByPredicate(Expression<Func<AspNetUser, bool>> predicate)
        {
            return _context.Users.Where(predicate).Count();
        }

        public AspNetUser FindById(string id)
        {
            return _context.Users.Where(m => m.Id == id).FirstOrDefault();
        }

        public IQueryable<AspNetUser> FindByPredicate(Expression<Func<AspNetUser, bool>> predicate)
        {
            return _context.Users.Where(predicate).AsQueryable();
        }

        public AspNetUser FindUser(string? Id)
        {
            return _context.Users.FirstOrDefault(m => m.Id == Id);
        }
        public AspNetUser FindByEmail(string? email)
        {

			AspNetUser user = _context.Users.Where(n => n.Email == email).FirstOrDefault();

            return user;
        }
    }
}
