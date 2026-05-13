using App.Services.Products.Create;
using App.Services.Products.Update;

namespace App.Services.Products;

public interface IProductService
{
    Task<ServiceResult<List<ProductResponse>>> GetTopPriceProductAsync(int count);
    Task<ServiceResult<ProductResponse?>> GetByIdAsync(int id);
    Task<ServiceResult<List<ProductResponse>>> GetAllListAsync();
    Task<ServiceResult<List<ProductResponse>>> GetPagedAllListAsync(int pageNumber, int pageSize);
    Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest request);
    Task<ServiceResult> UpdateAsync(int id, UpdateProductRequest request);
    Task<ServiceResult> UpdateStock(UpdateProductStockRequest request);
    Task<ServiceResult> DeleteAsync(int id);

}

