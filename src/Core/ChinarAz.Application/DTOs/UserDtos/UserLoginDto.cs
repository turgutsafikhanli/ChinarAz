namespace ChinarAz.Application.DTOs.UserDtos;

public record class UserLoginDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
