using App.Services.Categories.Create;
using App.Services.Categories.Dto;
using App.Services.Categories.Update;

namespace App.Services.Categories
{
    public interface ICategoryService
    {
        Task<ServiceResult<CategoryWithProductsResponse>> GetCategoryWithProductsAsync(int categoryId);
        Task<ServiceResult<List<CategoryWithProductsResponse>>> GetCategoryWithProductsAsync();
        Task<ServiceResult<List<CategoryResponse>>> GetAllAsync();
        Task<ServiceResult<CategoryResponse>> GetByIdAsync(int id);
        Task<ServiceResult<int>> CreateAsync(CreateCategoryRequest request);
        Task<ServiceResult> UpdateAsync(int id , UpdateCategoryRequest request);
        Task<ServiceResult> DeleteAsync(int id);

    }
}
