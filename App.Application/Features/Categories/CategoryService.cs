using System.Net;
using App.Application.Contracts.Persistence;
using App.Application.Features.Categories.Create;
using App.Application.Features.Categories.Dto;
using App.Application.Features.Categories.Update;
using App.Domain.Entities;
using AutoMapper;

namespace App.Application.Features.Categories;

public class CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork ,IMapper mapper) : ICategoryService
{
    // CRUD Operation

    public async Task<ServiceResult<CategoryWithProductsResponse>> GetCategoryWithProductsAsync(int categoryId)
    {
        var category = await categoryRepository.GetCategoriesWithProductsAsync(categoryId);
        if (category is null)
        {
            return ServiceResult<CategoryWithProductsResponse>.Fail("Kategori bulunamadı", HttpStatusCode.NotFound);
        }
        var categoryWithProductsResponse = mapper.Map<CategoryWithProductsResponse>(category);

        return ServiceResult<CategoryWithProductsResponse>.Success(categoryWithProductsResponse);
    }

    public async Task<ServiceResult<List<CategoryWithProductsResponse>>> GetCategoryWithProductsAsync()
    {
        var category = await categoryRepository.GetCategoryWithProductsAsync();
       
        var categoryWithProductsResponse = mapper.Map<List<CategoryWithProductsResponse>>(category);

        return ServiceResult<List<CategoryWithProductsResponse>>.Success(categoryWithProductsResponse);
    }

    public async Task<ServiceResult<List<CategoryResponse>>> GetAllAsync()
    {
        var categories = await categoryRepository.GetAllAsync();
        var categoryResponses = mapper.Map<List<CategoryResponse>>(categories);
        return ServiceResult<List<CategoryResponse>>.Success(categoryResponses);
    }

    public async Task<ServiceResult<CategoryResponse>> GetByIdAsync(int id)
    {
        var category = await categoryRepository.GetByIdAsync(id);
        if (category is null)
        {
            return ServiceResult<CategoryResponse>.Fail("Kategori bulunamadı", HttpStatusCode.NotFound);
        }
        var categoryResponse = mapper.Map<CategoryResponse>(category);
        return ServiceResult<CategoryResponse>.Success(categoryResponse);

      
    }

    public async Task<ServiceResult<int>> CreateAsync(CreateCategoryRequest request)
    {
        var anyCategory = await categoryRepository.AnyAsync(x => x.Name == request.Name);

        if (anyCategory)
        {
            return ServiceResult<int>.Fail("Kategori ismi veri tabanında bulnmaktadır.", HttpStatusCode.NotFound);
        }

        var newCategory = mapper.Map<Category>(request);
        await categoryRepository.AddAsync(newCategory);
        await unitOfWork.SaveChangesAsync();
        return ServiceResult<int>.SuccessAsCreate(newCategory.Id, $"api/categories/{newCategory.Id}");
    }

    public async Task<ServiceResult> UpdateAsync(int id, UpdateCategoryRequest request)
    {

        var isCategoryNameExist =
            await categoryRepository.AnyAsync(x => x.Name == request.Name && x.Id != id);

        if (isCategoryNameExist)
        {
            return ServiceResult.Fail("Kategori ismi veri tabanında bulunmaktadır", HttpStatusCode.BadRequest);
        }

        var category = mapper.Map<Category>(request);
        category.Id = id;
        categoryRepository.Update(category);
        await unitOfWork.SaveChangesAsync();

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var category = await categoryRepository.GetByIdAsync(id);
       
        categoryRepository.Delete(category!);
        await unitOfWork.SaveChangesAsync();
        return ServiceResult.Success(HttpStatusCode.NoContent);
    }



}


