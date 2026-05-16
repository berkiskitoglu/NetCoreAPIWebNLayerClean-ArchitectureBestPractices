using System.Net;
using App.Repositories;
using App.Repositories.Categories;
using App.Services.Categories.Create;
using App.Services.Categories.Dto;
using App.Services.Categories.Update;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace App.Services.Categories;

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
        var category = await categoryRepository.GetCategoryWithProducts().ToListAsync();
       
        var categoryWithProductsResponse = mapper.Map<List<CategoryWithProductsResponse>>(category);

        return ServiceResult<List<CategoryWithProductsResponse>>.Success(categoryWithProductsResponse);
    }

    public async Task<ServiceResult<List<CategoryResponse>>> GetAllAsync()
    {
        var categories = await categoryRepository.GetAll().ToListAsync();
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
        var anyCategory = await categoryRepository.Where(x => x.Name == request.Name).AnyAsync();

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
        var category = await categoryRepository.GetByIdAsync(id);
        if (category is null)
        {
            return ServiceResult.Fail("Güncellenecek kategori bulunamadı", HttpStatusCode.NotFound);
        }

        var isCategoryNameExist =
            await categoryRepository.Where(x => x.Name == request.Name && x.Id != category.Id).AnyAsync();

        if (isCategoryNameExist)
        {
            return ServiceResult.Fail("Kategori ismi veri tabanında bulunmaktadır", HttpStatusCode.BadRequest);
        }

        category = mapper.Map(request, category);
        categoryRepository.Update(category);
        await unitOfWork.SaveChangesAsync();

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var category = await categoryRepository.GetByIdAsync(id);
        if (category is null)
        {
            return ServiceResult.Fail("Silinecek kategori bulunamadı", HttpStatusCode.NotFound);
        }
        categoryRepository.Delete(category);
        await unitOfWork.SaveChangesAsync();
        return ServiceResult.Success(HttpStatusCode.NoContent);
    }



}


