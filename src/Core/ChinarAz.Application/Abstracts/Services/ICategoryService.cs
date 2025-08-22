using ChinarAz.Application.DTOs.CategoryDtos;
using ChinarAz.Application.Shared;

namespace ChinarAz.Application.Abstracts.Services;

public interface ICategoryService
{
    Task<BaseResponse<string>> AddAsync(CategoryCreateDto dto);
    Task<BaseResponse<string>> DeleteAsync(Guid id);
    Task<BaseResponse<CategoryUpdateDto>> UpdateAsync(CategoryUpdateDto dto);
    Task<BaseResponse<CategoryGetDto>> GetByIdAsync(Guid id);
    Task<BaseResponse<List<CategoryGetDto>>> GetAllAsync();
    Task<BaseResponse<List<CategoryGetDto>>> GetByNameSearchAsync(string namePart);
}
