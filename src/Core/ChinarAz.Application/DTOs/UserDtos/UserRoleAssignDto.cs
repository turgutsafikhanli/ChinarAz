namespace ChinarAz.Application.DTOs.UserDtos;

public record class UserRoleAssignDto
{
    public string UserId { get; set; } = null!;
    public List<string> RoleName { get; set; } = null!;
}
