using ChinarAz.Application.DTOs.FavouriteDtos;
using ChinarAz.Application.Shared;

namespace ChinarAz.Application.Abstracts.Services;

public interface IFavouriteService
{
    Task<BaseResponse<string>> AddAsync(FavouriteCreateDto dto);
    Task<BaseResponse<string>> RemoveAsync(Guid productId);
    Task<BaseResponse<List<FavouriteGetDto>>> GetUserFavouritesAsync();
}
