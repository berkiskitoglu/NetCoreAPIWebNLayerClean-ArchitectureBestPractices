using App.Domain.Entities;

namespace App.Application.Contracts.Persistence;

public interface ICategoryRepository : IGenericRepository<Category,int>
{
    Task<Category?> GetCategoriesWithProductsAsync(int id);
    Task<List<Category>> GetCategoryWithProductsAsync();
}

