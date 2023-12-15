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
    public class BranchRepository : GenericRepository<Branch, RefferalLinksDbContext, ApplicationUser>, IBranchRepository
    {
        public BranchRepository(RefferalLinksDbContext unitOfWork) : base(unitOfWork)
        {

        }
    }
}
