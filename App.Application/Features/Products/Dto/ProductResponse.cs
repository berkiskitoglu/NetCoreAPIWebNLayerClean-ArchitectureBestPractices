namespace App.Application.Features.Products.Dto;

public record ProductResponse(int Id , string Name , decimal Price, int Stock , int CategoryId);

