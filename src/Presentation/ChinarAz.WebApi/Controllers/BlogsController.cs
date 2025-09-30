using ChinarAz.Application.Abstracts.Services;
using ChinarAz.Application.DTOs.BlogDtos;
using ChinarAz.Application.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChinarAz.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogsController : ControllerBase
{
    private readonly IBlogService _blogService;

    public BlogsController(IBlogService blogService)
    {
        _blogService = blogService;
    }

    [HttpPost]
    [Authorize(Policy = Permissions.Blog.Create)]
    public async Task<IActionResult> Create([FromForm] BlogCreateDto dto)
    {
        var result = await _blogService.CreateAsync(dto);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPut]
    [Authorize(Policy = Permissions.Blog.Update)]
    public async Task<IActionResult> Update([FromForm] BlogUpdateDto dto)
    {
        var result = await _blogService.UpdateAsync(dto);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Permissions.Blog.Delete)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _blogService.DeleteAsync(id);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = Permissions.Blog.Get)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _blogService.GetByIdAsync(id);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _blogService.GetAllAsync();
        return StatusCode((int)result.StatusCode, result);
    }
}
