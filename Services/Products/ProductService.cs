using App.Repositories;
using App.Repositories.Products;
using App.Services.Products.Create;
using App.Services.Products.Update;
using Microsoft.EntityFrameworkCore;
using System.Net;
using App.Services.ExceptionHandler;
using AutoMapper;

namespace App.Services.Products;

public class ProductService(IProductRepository productRepository , IUnitOfWork unitOfWork , IMapper mapper) : IProductService
{
    public async Task<ServiceResult<List<ProductResponse>>> GetTopPriceProductAsync(int count)
    {
        var products = await productRepository.GetTopPriceProductAsync(count);

        #region ManuelMapping

        // var productsAsDto = products.Select(p => new ProductResponse(p.Id, p.Name, p.Price, p.Stock)).ToList();


        #endregion

        var productsAsDto = mapper.Map<List<ProductResponse>>(products);

        return new ServiceResult<List<ProductResponse>>()
        {
            Data = productsAsDto
        };
    }

    public async Task<ServiceResult<List<ProductResponse>>> GetAllListAsync()
    {
        var products = await productRepository.GetAll().ToListAsync();

        #region ManuelMapping

        // var productsAsDto = products.Select(p => new ProductResponse(p.Id, p.Name, p.Price, p.Stock)).ToList();


        #endregion

        var productsAsDto = mapper.Map<List<ProductResponse>>(products);

        return ServiceResult<List<ProductResponse>>.Success(productsAsDto);
    }

    public async Task<ServiceResult<List<ProductResponse>>> GetPagedAllListAsync(int pageNumber, int pageSize)

    {
        var products = await productRepository.GetAll().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        #region ManuelMapping

        // var productsAsDto = products.Select(p => new ProductResponse(p.Id, p.Name, p.Price, p.Stock)).ToList();


        #endregion

        var productsAsDto = mapper.Map<List<ProductResponse>>(products);

        return ServiceResult<List<ProductResponse>>.Success(productsAsDto);
    }

    public async Task<ServiceResult<ProductResponse?>> GetByIdAsync(int id)
    {
        var product = await productRepository.GetByIdAsync(id);

        if(product is null)
        {
          return  ServiceResult<ProductResponse?>.Fail("Product not Found", HttpStatusCode.NotFound);
        }

        #region ManuelMapping

        // var productsAsDto = products.Select(p => new ProductResponse(p.Id, p.Name, p.Price, p.Stock)).ToList();


        #endregion


        var productAsDto = mapper.Map<ProductResponse>(product);

        return ServiceResult<ProductResponse>.Success(productAsDto)!;
    }

    public async Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest request)
    {

        var anyProduct = await productRepository.Where(x => x.Name == request.Name).AnyAsync();

        if (anyProduct)
        {
            return ServiceResult<CreateProductResponse>.Fail("Ürün ismi veri tabanında bulnmaktadır.", HttpStatusCode.BadRequest);
        }

        var product = new Product()
        {
            Name = request.Name,
            Price = request.Price,
            Stock = request.Stock
        };

        await productRepository.AddAsync(product);
        await unitOfWork.SaveChangesAsync();
        return ServiceResult<CreateProductResponse>.SuccessAsCreate(new CreateProductResponse(product.Id),$"api/products/{product.Id}");
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
        return ServiceResult.Success(HttpStatusCode.NoContent);
    }

    public async Task<ServiceResult> UpdateStock(UpdateProductStockRequest request)
    {
        var product = await productRepository.GetByIdAsync(request.ProductId);
        if (product is null)
        {
            return ServiceResult.Fail("Product Not Found", HttpStatusCode.NotFound);
        }

        product.Stock = request.Quantity;
        productRepository.Update(product);
        await unitOfWork.SaveChangesAsync();
        return ServiceResult.Success(HttpStatusCode.NoContent);

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

