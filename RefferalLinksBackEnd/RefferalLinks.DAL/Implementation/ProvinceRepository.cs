using Maynghien.Common.Repository;
using RefferalLinks.DAL.Contract;
using RefferalLinks.DAL.Models.Context;
using RefferalLinks.DAL.Models.Entity;

namespace RefferalLinks.DAL.Implementation
{
	public class ProvinceRepository : GenericRepository<Province, RefferalLinksDbContext, ApplicationUser>, IProvinceRepository
	{
		public ProvinceRepository(RefferalLinksDbContext unitOfWork) : base(unitOfWork)
		{
		}
	}
}
