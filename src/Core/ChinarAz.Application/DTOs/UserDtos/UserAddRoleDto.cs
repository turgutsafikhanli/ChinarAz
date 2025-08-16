namespace ChinarAz.Application.DTOs.UserDtos;

public record class UserAddRoleDto
{
    public Guid UserId { get; set; }
    public List<Guid> RolesId { get; set; }

}
