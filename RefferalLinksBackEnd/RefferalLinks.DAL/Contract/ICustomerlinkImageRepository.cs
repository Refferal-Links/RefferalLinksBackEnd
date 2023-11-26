using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maynghien.Common.Repository;
using RefferalLinks.DAL.Models.Context;
using RefferalLinks.DAL.Models.Entity;

namespace RefferalLinks.DAL.Contract
{
    public interface ICustomerlinkImageRepository : IGenericRepository<CustomerLinkImage, RefferalLinksDbContext, ApplicationUser>
    {
    }
}
