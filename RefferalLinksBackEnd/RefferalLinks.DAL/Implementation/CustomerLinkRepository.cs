using Maynghien.Common.Repository;
using RefferalLinks.DAL.Contract;
using RefferalLinks.DAL.Models.Context;
using RefferalLinks.DAL.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefferalLinks.DAL.Implementation
{
    public class CustomerLinkRepository : GenericRepository<Customerlink, RefferalLinksDbContext, ApplicationUser> , ICustomerLinkRepository
    {
        public CustomerLinkRepository(RefferalLinksDbContext unitOfWork) : base(unitOfWork)
        {
        }
    }
}
