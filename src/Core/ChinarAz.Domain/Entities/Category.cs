namespace ChinarAz.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = null!;
    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<Blog> Blogs { get; set; } = new List<Blog>();
}
