using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Data
{
	public class ProductRepository(DataContext context) : Repository<Product>(context)
	{
		public override IEnumerable<Product> GetAll()
		{
			//AIM: Products with Categories:

			//1) fetch from db:
			return context.Products.Include(p => p.Category);
		}

		public override Product? GetById(int id)
		{
			return context.Products.Include(p => p.Category).SingleOrDefault(p => p.Id == id);
		}
	}
}
