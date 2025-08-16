using Microsoft.AspNetCore.Identity;

namespace ChinarAz.Domain.Entities;

public class AppUser : IdentityUser
{
    public string Fullname { get; set; } = null!;
    public string? RefreshToken { get; set; }
    public DateTime ExpiryDate { get; set; }
}
