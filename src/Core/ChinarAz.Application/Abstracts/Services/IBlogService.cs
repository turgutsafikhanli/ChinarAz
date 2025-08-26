using ChinarAz.Application.DTOs.BlogDtos;
using ChinarAz.Application.Shared;

namespace ChinarAz.Application.Abstracts.Services;

public interface IBlogService
{
    Task<BaseResponse<string>> CreateAsync(BlogCreateDto dto);
    Task<BaseResponse<string>> UpdateAsync(BlogUpdateDto dto);
    Task<BaseResponse<string>> DeleteAsync(Guid id);
    Task<BaseResponse<BlogGetDto>> GetByIdAsync(Guid id);
    Task<BaseResponse<List<BlogGetDto>>> GetAllAsync();
}
