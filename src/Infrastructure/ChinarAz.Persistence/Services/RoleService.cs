using ChinarAz.Application.Abstracts.Services;
using ChinarAz.Application.DTOs.RoleDtos;
using ChinarAz.Application.Shared;
using ChinarAz.Application.Shared.Helpers;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Security.Claims;

namespace ChinarAz.Persistence.Services;

public class RoleService : IRoleService
{
    private RoleManager<IdentityRole> _roleManager { get; }

    public RoleService(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<BaseResponse<string?>> CreateRole(RoleCreateDto dto)
    {
        var existingRole = await _roleManager.FindByNameAsync(dto.Name);
        if (existingRole != null)
            return new BaseResponse<string?>("This role already exists", HttpStatusCode.BadRequest);

        var allPermissions = PermissionHelper.GetAllPermissionsList();
        var invalidPermissions = dto.PermissionsList.Except(allPermissions).ToList();

        if (invalidPermissions.Any())
            return new BaseResponse<string?>($"Invalid permissions: {string.Join(", ", invalidPermissions)}", HttpStatusCode.BadRequest);

        var newRole = new IdentityRole(dto.Name);
        var result = await _roleManager.CreateAsync(newRole);

        if (!result.Succeeded)
            return new BaseResponse<string?>(string.Join("; ", result.Errors.Select(e => e.Description)), HttpStatusCode.BadRequest);

        // Rolu yenidən yüklə (bəzən lazımdır)
        var role = await _roleManager.FindByNameAsync(dto.Name);

        foreach (var permission in dto.PermissionsList)
        {
            var claimResult = await _roleManager.AddClaimAsync(role, new Claim("Permission", permission));
            if (!claimResult.Succeeded)
                return new BaseResponse<string?>(string.Join("; ", claimResult.Errors.Select(e => e.Description)), HttpStatusCode.BadRequest);
        }

        return new BaseResponse<string?>("Role created successfully", true, HttpStatusCode.Created);
    }
    public async Task<BaseResponse<bool>> DeleteRoleAsync(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            return new BaseResponse<bool>("Rol tapılmadı.", false, HttpStatusCode.NotFound);
        }

        var result = await _roleManager.DeleteAsync(role);
        if (!result.Succeeded)
        {
            return new BaseResponse<bool>("Rol silinərkən xəta baş verdi.", false, HttpStatusCode.InternalServerError);
        }

        return new BaseResponse<bool>("Rol uğurla silindi.", true, HttpStatusCode.OK);
    }
    public async Task<BaseResponse<bool>> UpdateRoleAsync(RoleUpdateDto dto)
    {
        var role = await _roleManager.FindByNameAsync(dto.RoleName);
        if (role == null)
            return new BaseResponse<bool>("Rol tapılmadı.", false, HttpStatusCode.NotFound);

        var existingClaims = await _roleManager.GetClaimsAsync(role);
        var existingPermissionValues = existingClaims.Select(c => c.Value).ToHashSet();

        var newClaims = dto.NewPermissions
            .Where(p => !existingPermissionValues.Contains(p))
            .Select(p => new Claim("Permission", p))
            .ToList();

        foreach (var claim in newClaims)
        {
            var result = await _roleManager.AddClaimAsync(role, claim);
            if (!result.Succeeded)
                return new BaseResponse<bool>("Permission əlavə edilərkən xəta baş verdi.", false, HttpStatusCode.InternalServerError);
        }

        return new BaseResponse<bool>("Rol səlahiyyətləri uğurla yeniləndi.", true, HttpStatusCode.OK);
    }

}
