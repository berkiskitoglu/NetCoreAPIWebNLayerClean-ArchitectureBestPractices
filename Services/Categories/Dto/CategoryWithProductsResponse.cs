using App.Services.Products;

namespace App.Services.Categories.Dto;

public record CategoryWithProductsResponse(int Id, string Name, List<ProductResponse> Products);


