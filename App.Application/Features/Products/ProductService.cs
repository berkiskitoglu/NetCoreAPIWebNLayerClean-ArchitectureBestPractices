using System.Net;
using App.Application.Contracts.Caching;
using App.Application.Contracts.Persistence;
using App.Application.Features.Products.Create;
using App.Application.Features.Products.Dto;
using App.Application.Features.Products.Update;
using App.Application.Features.Products.UpdateStock;
using App.Domain.Entities;
using AutoMapper;

namespace App.Application.Features.Products;

public class ProductService(IProductRepository productRepository , IUnitOfWork unitOfWork , IMapper mapper , ICacheService cacheService) : IProductService
{
    private const string ProdctListCacheKey = "product-list";
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

        //cache aside design pattern

        // 1. any cache
        // 2. from db
        // 3. caching data


        var productListAsCached = await cacheService.GetAsync<List<ProductResponse>>(ProdctListCacheKey);
        if(productListAsCached != null)
        {
            return ServiceResult<List<ProductResponse>>.Success(productListAsCached);
        }

        var products = await productRepository.GetAllAsync();

        #region ManuelMapping

        // var productsAsDto = products.Select(p => new ProductResponse(p.Id, p.Name, p.Price, p.Stock)).ToList();


        #endregion

        var productsAsDto = mapper.Map<List<ProductResponse>>(products);

        await cacheService.AddAsync(ProdctListCacheKey, productsAsDto, TimeSpan.FromMinutes(5));

        return ServiceResult<List<ProductResponse>>.Success(productsAsDto);
    }

    public async Task<ServiceResult<List<ProductResponse>>> GetPagedAllListAsync(int pageNumber, int pageSize)

    {
        var products = await productRepository.GetAllPagedAsync(pageNumber, pageSize);

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
          return  ServiceResult<ProductResponse?>.Fail("Güncellenecek ürün bulunamadı.", HttpStatusCode.NotFound);
        }

        #region ManuelMapping

        // var productsAsDto = products.Select(p => new ProductResponse(p.Id, p.Name, p.Price, p.Stock)).ToList();


        #endregion


        var productAsDto = mapper.Map<ProductResponse>(product);

        return ServiceResult<ProductResponse>.Success(productAsDto)!;
    }

    public async Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest request)
    {

        var anyProduct = await productRepository.AnyAsync(x => x.Name == request.Name);

        if (anyProduct)
        {
            return ServiceResult<CreateProductResponse>.Fail("Ürün ismi veri tabanında bulnmaktadır.", HttpStatusCode.BadRequest);
        }

        var product = mapper.Map<Product>(request);

        //var product = new Product()
        //{
        //    Name = request.Name,
        //    Price = request.Price,
        //    Stock = request.Stock
        //};

        await productRepository.AddAsync(product);
        await unitOfWork.SaveChangesAsync();
        return ServiceResult<CreateProductResponse>.SuccessAsCreate(new CreateProductResponse(product.Id),$"api/products/{product.Id}");
    }

    public async Task<ServiceResult> UpdateAsync(int id , UpdateProductRequest request)
    {

        // Fast Fail Önce başarısız durumları ele al
        // Guard Clauses Önce tüm olumsuz durumları if if le yaz else yazma

        var isProductNameExist = await productRepository.AnyAsync(x => x.Name == request.Name && x.Id != id);

        if (isProductNameExist)
        {
            return ServiceResult.Fail("Ürün ismi veri tabanında bulnmaktadır.", HttpStatusCode.BadRequest);
        }



        var product = mapper.Map<Product>(request);
        product.Id = id;

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
        productRepository.Delete(product!);
        await unitOfWork.SaveChangesAsync();
        return ServiceResult.Success();
    }
}

