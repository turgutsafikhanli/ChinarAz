using ChinarAz.Application.Abstracts.Services;
using ChinarAz.Application.DTOs.RoleDtos;
using ChinarAz.Application.Shared;
using ChinarAz.Application.Shared.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChinarAz.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private IRoleService _roleService { get; }

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }
    // GET: api/<RolesController>
    [HttpGet("permissions")]
    [Authorize(Policy = Permissions.Role.GetAllPermissions)]
    public IActionResult GetAllPermissions()
    {
        var permissions = PermissionHelper.GetAllPermissions();
        return Ok(permissions);

    }
    [HttpPost]
    [Authorize(Policy = Permissions.Role.Create)]
    public async Task<IActionResult> CreateRole(RoleCreateDto dto)
    {
        var result = await _roleService.CreateRole(dto);
        return StatusCode((int)result.StatusCode, result);
    }
    [HttpDelete("{name}")]
    [Authorize(Policy = Permissions.Role.Delete)]
    public async Task<IActionResult> DeleteRole(string name)
    {
        var result = await _roleService.DeleteRoleAsync(name);
        return StatusCode((int)result.StatusCode, result);
    }
    [HttpPut]
    [Authorize(Policy = Permissions.Role.Update)]
    public async Task<IActionResult> UpdateRole([FromBody] RoleUpdateDto dto)
    {
        var result = await _roleService.UpdateRoleAsync(dto);
        return StatusCode((int)result.StatusCode, result);
    }
}
