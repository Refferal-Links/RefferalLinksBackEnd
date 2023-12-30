using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RefferalLinks.DAL.Contract;
using RefferalLinks.DAL.Migrations;
using RefferalLinks.DAL.Models.Context;
using RefferalLinks.DAL.Models.Entity;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

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

        public void Edit(ApplicationUser user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(string Id)
        {
            //const int batchSize = 100;
            var user =_context.Users.First(x=>x.Id == Id);
            var customersToDelete = _context.Customer
                .Where(c => c.ApplicationUserId == Id || c.CSKHId == Id)
                //.Take(batchSize)
                .ToList();
            foreach(var i in customersToDelete)
            {
                var listCustomerLink = _context.Customerlink.Where(cl => cl.CustomerId == i.Id).ToList();
                foreach(var c in listCustomerLink)
                {
                    var ListCustomerLinkImage = _context.CustomerlinkImage.Where(x => x.CustomerLinkId == c.Id).ToList();
                    _context.CustomerlinkImage.RemoveRange(ListCustomerLinkImage);
                }
                _context.Customerlink.RemoveRange(listCustomerLink);
                
            }
            _context.Customer.RemoveRange(customersToDelete);
            
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public ApplicationUser UserWithCustomerCount()
        {
            var role = _context.Roles.Where(x => x.Name == "Sale").First();
            var userWithMinCustomers = _context.Users
            .Where(x=>x.IsReceiveAllocation == true && x.LockoutEnabled == false)
            .Select(user => new
            {
                User = user,
                CustomerCount = _context.Customer.Count(customer => customer.CreatedOn.Value.Month == DateTime.UtcNow.Month && customer.CreatedOn.Value.Year == DateTime.UtcNow.Year && customer.ApplicationUserId == user.Id),
            })
            .Where(m => _context.UserRoles.Where(r => r.RoleId == role.Id).Select(r => r.UserId).Contains(m.User.Id))
            .OrderBy(entry => entry.CustomerCount)
            .FirstOrDefault();
            var result = userWithMinCustomers.User;
            return result;
;
        }
    }
}
