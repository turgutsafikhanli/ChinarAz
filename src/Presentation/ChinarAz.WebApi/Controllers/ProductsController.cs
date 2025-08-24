using ChinarAz.Application.Abstracts.Services;
using ChinarAz.Application.DTOs.ProductDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChinarAz.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    public ProductsController(IProductService productService) => _productService = productService;

    [HttpPost]
    [Authorize(Policy = "Product.Create")]
    public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
    {
        var result = await _productService.CreateAsync(dto);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPut]
    [Authorize(Policy = "Product.Update")]
    public async Task<IActionResult> Update([FromBody] ProductUpdateDto dto)
    {
        var result = await _productService.UpdateAsync(dto);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Product.Delete")]
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
