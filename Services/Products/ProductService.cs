using App.Repositories;
using App.Repositories.Products;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace App.Services.Products;

public class ProductService(IProductRepository productRepository , IUnitOfWork unitOfWork) : IProductService
{
    public async Task<ServiceResult<List<ProductResponse>>> GetTopPriceProductAsync(int count)
    {
        var products = await productRepository.GetTopPriceProductAsync(count);

        var productsAsDto = products.Select(p => new ProductResponse(p.Id, p.Name, p.Price, p.Stock)).ToList();
    

        return new ServiceResult<List<ProductResponse>>()
        {
            Data = productsAsDto
        };
    }

    public async Task<ServiceResult<List<ProductResponse>>> GetAllListAsync()
    {
        var products = await productRepository.GetAll().ToListAsync();
        var productsAsDto = products.Select(p => new ProductResponse(p.Id, p.Name, p.Price, p.Stock)).ToList();
        return ServiceResult<List<ProductResponse>>.Success(productsAsDto);
    }

    public async Task<ServiceResult<ProductResponse?>> GetByIdAsync(int id)
    {
        var product = await productRepository.GetByIdAsync(id);

        if(product is null)
        {
            ServiceResult<CreateProductResponse>.Fail("Product not Found", HttpStatusCode.NotFound);
        }

        var productAsDto = new ProductResponse(product!.Id, product.Name, product.Price, product.Stock);

        return ServiceResult<ProductResponse>.Success(productAsDto)!;
    }

    public async Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest request)
    {
        var product = new Product()
        {
            Name = request.Name,
            Price = request.Price,
            Stock = request.Stock
        };

        await productRepository.AddAsync(product);
        await unitOfWork.SaveChangesAsync();
        return ServiceResult<CreateProductResponse>.Success(new CreateProductResponse(product.Id));
    }

    public async Task<ServiceResult> UpdateAsync(int id , UpdateProductRequest request)
    {

        // Fast Fail Önce başarısız durumları ele al
        // Guard Clauses Önce tüm olumsuz durumları if if le yaz else yazma

        var product = await productRepository.GetByIdAsync(id);

        if(product is null)
        {
            return ServiceResult.Fail("Product not found", HttpStatusCode.NotFound);
        }
        product.Name = request.Name;
        product.Price = request.Price;
        product.Stock = request.Stock;

        productRepository.Update(product);
        await unitOfWork.SaveChangesAsync();
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var product = await productRepository.GetByIdAsync(id);

        if(product is null)
        {
            return ServiceResult.Fail("Product not found", HttpStatusCode.NotFound);
        }

        productRepository.Delete(product);
        await unitOfWork.SaveChangesAsync();
        return ServiceResult.Success();
    }
}

