using ChinarAz.Application.Abstracts.Repositories;
using ChinarAz.Application.Abstracts.Services;
using ChinarAz.Application.DTOs.FavouriteDtos;
using ChinarAz.Application.Shared;
using ChinarAz.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Claims;

namespace ChinarAz.Persistence.Services;

public class FavouriteService : IFavouriteService
{
    private readonly IFavouriteRepository _favouriteRepository;
    private readonly IProductRepository _productRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FavouriteService(
        IFavouriteRepository favouriteRepository,
        IProductRepository productRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _favouriteRepository = favouriteRepository;
        _productRepository = productRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    private string GetUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("User is not authenticated.");
        return userId;
    }

    public async Task<BaseResponse<string>> AddAsync(FavouriteCreateDto dto)
    {
        try
        {
            var userId = GetUserId();

            var product = await _productRepository.GetByIdAsync(dto.ProductId);
            if (product == null)
                return new BaseResponse<string>("Product not found", HttpStatusCode.NotFound);

            var existing = await _favouriteRepository.GetByUserAndProductAsync(userId, dto.ProductId);
            if (existing != null)
                return new BaseResponse<string>("Already in favourites", HttpStatusCode.BadRequest);

            var favourite = new Favourite
            {
                UserId = userId,
                ProductId = dto.ProductId
            };

            await _favouriteRepository.AddAsync(favourite);
            await _favouriteRepository.SaveChangeAsync();

            return new BaseResponse<string>("Added to favourites", HttpStatusCode.Created)
            {
                Data = favourite.Id.ToString()
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<string>($"Error adding favourite: {ex.Message}", HttpStatusCode.InternalServerError);
        }
    }

    public async Task<BaseResponse<string>> RemoveAsync(Guid productId)
    {
        try
        {
            var userId = GetUserId();

            var favourite = await _favouriteRepository.GetByUserAndProductAsync(userId, productId);
            if (favourite == null)
                return new BaseResponse<string>("Favourite not found", HttpStatusCode.NotFound);

            _favouriteRepository.Delete(favourite);
            await _favouriteRepository.SaveChangeAsync();

            return new BaseResponse<string>("Removed from favourites", HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return new BaseResponse<string>($"Error removing favourite: {ex.Message}", HttpStatusCode.InternalServerError);
        }
    }

    public async Task<BaseResponse<List<FavouriteGetDto>>> GetUserFavouritesAsync()
    {
        try
        {
            var userId = GetUserId();

            var favourites = await _favouriteRepository.GetByUserIdAsync(userId);

            var dtoList = favourites.Select(f => new FavouriteGetDto
            {
                Id = f.Id,
                ProductId = f.ProductId,
                ProductName = f.Product.Name
            }).ToList();

            return new BaseResponse<List<FavouriteGetDto>>("User favourites fetched successfully", dtoList, HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return new BaseResponse<List<FavouriteGetDto>>($"Error fetching favourites: {ex.Message}", HttpStatusCode.InternalServerError);
        }
    }
}
