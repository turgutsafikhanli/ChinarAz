using ChinarAz.Application.Abstracts.Services;
using ChinarAz.Application.DTOs.CategoryDtos;
using ChinarAz.Application.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChinarAz.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    private ICategoryService _categoryService { get; }

    // POST api/<CategoriesController>
    [HttpPost]
    [Authorize(Policy = Permissions.Category.Create)]
    [ProducesResponseType(typeof(BaseResponse<CategoryUpdateDto>), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Post([FromBody] CategoryCreateDto dto)
    {
        var result = await _categoryService.AddAsync(dto);
        return StatusCode((int)result.StatusCode, result);
    }

    // PUT api/<CategoriesController>/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] CategoryUpdateDto dto)
    {
        var result = await _categoryService.UpdateAsync(dto);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet()]
    [Authorize(Policy = Permissions.Category.Get)]
    [ProducesResponseType(typeof(BaseResponse<CategoryGetDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetByIdAsync([FromQuery] Guid id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        return StatusCode((int)category.StatusCode, category);
    }

    [HttpGet("getall")]
    [Authorize(Policy = Permissions.Category.Get)]
    [ProducesResponseType(typeof(BaseResponse<CategoryGetDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetAllAsync()
    {
        var categories = await _categoryService.GetAllAsync();
        return StatusCode((int)categories.StatusCode, categories);
    }

    // DELETE api/<CategoriesController>/5
    [HttpDelete("{id}")]
    [Authorize(Policy = Permissions.Category.Delete)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _categoryService.DeleteAsync(id);
        return StatusCode((int)result.StatusCode, result);
    }
    [HttpGet("search")]
    [Authorize(Policy = Permissions.Category.Get)]
    [ProducesResponseType(typeof(BaseResponse<List<CategoryGetDto>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BaseResponse<string>), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetByNameSearchAsync(string search)
    {
        var category = await _categoryService.GetByNameSearchAsync(search);
        return StatusCode((int)category.StatusCode, category);
    }
}
