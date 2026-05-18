using App.Application.Features.Products.Dto;

namespace App.Application.Features.Categories.Dto;

public record CategoryWithProductsResponse(int Id, string Name, List<ProductResponse> Products);


