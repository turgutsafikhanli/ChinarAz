using ChinarAz.Application.Abstracts.Services;
using ChinarAz.Application.DTOs.ProductDtos;
using ChinarAz.Application.Shared;
using ChinarAz.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChinarAz.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IFileUploadService _fileUploadService;
    public ProductsController(IProductService productService, IFileUploadService fileUploadService)
    {
        _productService = productService;
        _fileUploadService = fileUploadService;
    }

    [HttpPost]
    [Authorize(Policy = Permissions.Product.Create)]
    public async Task<IActionResult> Create([FromForm] ProductCreateDto dto)
    {
        if (dto.Images == null || !dto.Images.Any())
            return BadRequest(new BaseResponse<string>(HttpStatusCode.BadRequest)
            {
                Success = false,
                Message = "No images provided"
            });

        var uploadedImageUrls = new List<string>();

        foreach (var image in dto.Images)
        {
            var url = await _fileUploadService.UploadAsync(image);
            uploadedImageUrls.Add(url);
        }

        dto.ImageUrls = uploadedImageUrls;

        var result = await _productService.CreateAsync(dto);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPut]
    [Authorize(Roles = Permissions.Product.Update)]
    public async Task<IActionResult> Update(Guid id, [FromForm] ProductUpdateDto dto)
    {
        if (dto.Images != null && dto.Images.Any())
        {
            var uploadedImageUrls = new List<string>();

            foreach (var image in dto.Images)
            {
                var url = await _fileUploadService.UploadAsync(image);
                uploadedImageUrls.Add(url);
            }

            dto.ImageUrls = uploadedImageUrls;
        }

        var result = await _productService.UpdateAsync(dto);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Permissions.Product.Delete)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _productService.DeleteAsync(id);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _productService.GetByIdAsync(id);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("category/{categoryId}")]
    public async Task<IActionResult> GetByCategoryId(Guid categoryId)
    {
        var result = await _productService.GetByCategoryIdAsync(categoryId);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(string keyword)
    {
        var result = await _productService.SearchAsync(keyword);
        return StatusCode((int)result.StatusCode, result);
    }
}
