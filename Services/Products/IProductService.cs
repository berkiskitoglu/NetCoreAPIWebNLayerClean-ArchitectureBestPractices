namespace App.Services.Products;

public interface IProductService
{
    Task<ServiceResult<List<ProductResponse>>> GetTopPriceProductAsync(int count);
    Task<ServiceResult<ProductResponse?>> GetByIdAsync(int id);
    Task<ServiceResult<List<ProductResponse>>> GetAllListAsync();
    Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest request);
    Task<ServiceResult> UpdateAsync(int id, UpdateProductRequest request);
    Task<ServiceResult> DeleteAsync(int id);

}

