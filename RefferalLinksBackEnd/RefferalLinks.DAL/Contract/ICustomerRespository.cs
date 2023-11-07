using Maynghien.Common.Repository;
using RefferalLinks.DAL.Models.Context;
using RefferalLinks.DAL.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefferalLinks.DAL.Contract
{
    public interface ICustomerRespository : IGenericRepository<Customer, RefferalLinksDbContext, ApplicationUser>
    {
    }
}
