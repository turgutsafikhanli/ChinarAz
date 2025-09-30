using ChinarAz.Application.Abstracts.Services;
using ChinarAz.Application.DTOs.FavouriteDtos;
using ChinarAz.Application.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChinarAz.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FavouritesController : ControllerBase
{
    private readonly IFavouriteService _favouriteService;

    public FavouritesController(IFavouriteService favouriteService)
    {
        _favouriteService = favouriteService;
    }

    // Yeni favourite əlavə et
    [HttpPost("add")]
    [Authorize]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Add([FromBody] FavouriteCreateDto dto)
    {
        var result = await _favouriteService.AddAsync(dto);
        return StatusCode((int)result.StatusCode, result);
    }

    // Favourite sil
    [HttpDelete("remove/{productId:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Remove(Guid productId)
    {
        var result = await _favouriteService.RemoveAsync(productId);
        return StatusCode((int)result.StatusCode, result);
    }

    // İstifadəçinin favourites-lərini gətir
    [HttpGet("user")]
    [Authorize]
    public async Task<IActionResult> GetUserFavourites()
    {
        var result = await _favouriteService.GetUserFavouritesAsync();
        return StatusCode((int)result.StatusCode, result);
    }
}
