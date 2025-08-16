namespace ChinarAz.Application.DTOs.RoleDtos;

public record class RoleUpdateDto
{
    public string RoleName { get; set; } = null!;
    public List<string> NewPermissions { get; set; } = new();
}
