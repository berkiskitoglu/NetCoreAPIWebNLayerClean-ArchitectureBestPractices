namespace App.Repositories.Categories;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<Category?> GetCategoriesWithProductsAsync(int id);
    IQueryable<Category?> GetCategoryWithProducts();
}

