using AutoMapper;
using ChinarAz.Application.Abstracts.Repositories;
using ChinarAz.Application.Abstracts.Services;
using ChinarAz.Application.DTOs.CategoryDtos;
using ChinarAz.Application.Shared;
using ChinarAz.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ChinarAz.Persistence.Services;

public class CategoryService : ICategoryService
{
    private ICategoryRepository _categoryRepository { get; }
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<BaseResponse<string>> AddAsync(CategoryCreateDto dto)
    {
        var categoryDb = await _categoryRepository.GetByFiltered(c => c.Name.Trim().ToLower() == dto.Name.Trim().ToLower()).FirstOrDefaultAsync();
        if (categoryDb is not null)
        {
            return new BaseResponse<string>("This category already exists", System.Net.HttpStatusCode.BadRequest);
        }
        Category category = new()
        {
            Name = dto.Name.Trim()
        };
        await _categoryRepository.AddAsync(category);
        await _categoryRepository.SaveChangeAsync();
        return new BaseResponse<string>(System.Net.HttpStatusCode.Created);
    }

    public async Task<BaseResponse<string>> DeleteAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category is null)
            return new BaseResponse<string>("Category not found", HttpStatusCode.NotFound);

        await _categoryRepository.SoftDeleteAsync(category);

        return new BaseResponse<string>("Category soft-deleted successfully", HttpStatusCode.OK);
    }

    public async Task<BaseResponse<List<CategoryGetDto>>> GetAllAsync()
    {
        var categories = _categoryRepository.GetAll();
        var dtoList = new List<CategoryGetDto>();
        foreach (var category in categories)
        {
            dtoList.Add(new CategoryGetDto
            {
                Id = category.Id,
                Name = category.Name
            });
        }
        return new BaseResponse<List<CategoryGetDto>>("Data", dtoList, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<CategoryGetDto>> GetByIdAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category is null)
        {
            return new BaseResponse<CategoryGetDto>(HttpStatusCode.NotFound);
        }
        var dto = new CategoryGetDto
        {
            Id = category.Id,
            Name = category.Name
        };
        return new BaseResponse<CategoryGetDto>("Data", dto, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<CategoryUpdateDto>> UpdateAsync(CategoryUpdateDto dto)
    {
        var categoryDb = await _categoryRepository.GetByIdAsync(dto.Id);
        if (categoryDb is null)
        {
            return new BaseResponse<CategoryUpdateDto>(HttpStatusCode.NotFound);
        }

        var existedCategory = await _categoryRepository
            .GetByFiltered(c => c.Name.Trim().ToLower() == dto.Name.Trim().ToLower() && c.Id != dto.Id)
            .FirstOrDefaultAsync();
        if (existedCategory is not null)
        {
            return new BaseResponse<CategoryUpdateDto>("This category already exists", HttpStatusCode.BadRequest);
        }
        categoryDb.Name = dto.Name.Trim();

        await _categoryRepository.SaveChangeAsync();
        return new BaseResponse<CategoryUpdateDto>("Category updated successfully", dto, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<List<CategoryGetDto>>> GetByNameSearchAsync(string namePart)
    {
        var categories = await _categoryRepository.GetByNameSearchAsync(namePart);
        if (categories == null || !categories.Any())
        {
            return new BaseResponse<List<CategoryGetDto>>("No categories found with the given name part", HttpStatusCode.NotFound);
        }

        // Manual mapping
        var dtoList = new List<CategoryGetDto>();
        foreach (var category in categories)
        {
            dtoList.Add(new CategoryGetDto
            {
                Id = category.Id,
                Name = category.Name
            });
        }

        return new BaseResponse<List<CategoryGetDto>>("Data", dtoList, HttpStatusCode.OK);
    }
}
