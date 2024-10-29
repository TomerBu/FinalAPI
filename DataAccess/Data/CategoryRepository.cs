using DataAccess.Models;

namespace DataAccess.Data
{
	public class CategoryRepository(DataContext context) : Repository<Category>(context)
	{
	}
}
