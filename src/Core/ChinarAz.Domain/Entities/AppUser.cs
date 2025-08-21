using Microsoft.AspNetCore.Identity;

namespace ChinarAz.Domain.Entities;

public class AppUser : IdentityUser
{
    public string Fullname { get; set; } = null!;
    public string? RefreshToken { get; set; }
    public DateTime ExpiryDate { get; set; }

    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<Favourite> Favourites { get; set; } = new List<Favourite>();
}
