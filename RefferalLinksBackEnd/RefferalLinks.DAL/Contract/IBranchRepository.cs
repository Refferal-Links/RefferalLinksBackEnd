using Maynghien.Common.Repository;
using RefferalLinks.DAL.Models.Context;
using RefferalLinks.DAL.Models.Entity;

namespace RefferalLinks.DAL.Contract
{
    public interface IBranchRepository : IGenericRepository<Branch, RefferalLinksDbContext, ApplicationUser>
    {
    }
}
