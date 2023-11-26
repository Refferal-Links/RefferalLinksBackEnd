using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maynghien.Common.Repository;
using RefferalLinks.DAL.Contract;
using RefferalLinks.DAL.Models.Context;
using RefferalLinks.DAL.Models.Entity;

namespace RefferalLinks.DAL.Implementation
{
    public class CustomerlinkImageRepository : GenericRepository<CustomerLinkImage, RefferalLinksDbContext, ApplicationUser>, ICustomerlinkImageRepository
    {
        public CustomerlinkImageRepository(RefferalLinksDbContext unitOfWork) : base(unitOfWork)
        {
            _context = unitOfWork;
        }
    }
}
