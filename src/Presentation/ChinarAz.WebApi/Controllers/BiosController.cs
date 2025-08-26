using ChinarAz.Application.Abstracts.Services;
using ChinarAz.Application.DTOs.BioDtos;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChinarAz.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BiosController : ControllerBase
{
    private readonly IBioService _bioService;

    public BiosController(IBioService bioService)
    {
        _bioService = bioService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BioCreateDto dto)
    {
        var result = await _bioService.CreateAsync(dto);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] BioUpdateDto dto)
    {
        var result = await _bioService.UpdateAsync(dto);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _bioService.DeleteAsync(id);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _bioService.GetByIdAsync(id);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _bioService.GetAllAsync();
        return StatusCode((int)result.StatusCode, result);
    }
}
