using ChinarAz.Application.DTOs.BioDtos;
using ChinarAz.Application.Shared;

namespace ChinarAz.Application.Abstracts.Services;

public interface IBioService
{
    Task<BaseResponse<string>> CreateAsync(BioCreateDto dto);
    Task<BaseResponse<string>> UpdateAsync(BioUpdateDto dto);
    Task<BaseResponse<string>> DeleteAsync(Guid id);
    Task<BaseResponse<BioGetDto>> GetByIdAsync(Guid id);
    Task<BaseResponse<List<BioGetDto>>> GetAllAsync();
}
