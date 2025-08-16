using ChinarAz.Application.DTOs.RoleDtos;
using ChinarAz.Application.Shared;

namespace ChinarAz.Application.Abstracts.Services;

public interface IRoleService
{
    Task<BaseResponse<string?>> CreateRole(RoleCreateDto dto);
    Task<BaseResponse<bool>> DeleteRoleAsync(string roleName);
    Task<BaseResponse<bool>> UpdateRoleAsync(RoleUpdateDto dto);

}
