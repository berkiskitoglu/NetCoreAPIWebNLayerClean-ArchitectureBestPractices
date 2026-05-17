using Microsoft.EntityFrameworkCore;

namespace App.Repositories.Categories;

public class CategoryRepository(AppDbContext context) : GenericRepostiory<Category,int>(context) , ICategoryRepository
{
    public Task<Category?> GetCategoriesWithProductsAsync(int id)
    {
      return Context.Categories.Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == id);
    }

    public IQueryable<Category?> GetCategoryWithProducts()
    {
        return Context.Categories.Include(x => x.Products).AsQueryable();
    }
}

