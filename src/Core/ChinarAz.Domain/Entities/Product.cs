namespace ChinarAz.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = null!;

    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public bool IsWeighted { get; set; }

    public ICollection<Image> Images { get; set; } = new List<Image>();
    public ICollection<Favourite> Favourites { get; set; } = new List<Favourite>();
    public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
}