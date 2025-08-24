using ChinarAz.Application.DTOs.ProductDtos;
using ChinarAz.Application.Shared;

namespace ChinarAz.Application.Abstracts.Services;

public interface IProductService
{
    // Admin metodları
    Task<BaseResponse<string>> CreateAsync(ProductCreateDto dto);
    Task<BaseResponse<string>> UpdateAsync(ProductUpdateDto dto);
    Task<BaseResponse<string>> DeleteAsync(Guid id);

    // Müştəri metodları
    Task<BaseResponse<ProductGetDto>> GetByIdAsync(Guid id);
    Task<BaseResponse<List<ProductGetDto>>> GetByCategoryIdAsync(Guid categoryId);
    Task<BaseResponse<List<ProductGetDto>>> SearchAsync(string keyword);
}
