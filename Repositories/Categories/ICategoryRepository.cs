namespace App.Repositories.Categories;

public interface ICategoryRepository : IGenericRepository<Category,int>
{
    Task<Category?> GetCategoriesWithProductsAsync(int id);
    IQueryable<Category?> GetCategoryWithProducts();
}

