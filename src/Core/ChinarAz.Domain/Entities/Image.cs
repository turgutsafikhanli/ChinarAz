namespace ChinarAz.Domain.Entities;

public class Image : BaseEntity
{
    public string ImageUrl { get; set; } = null!;
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public Guid BlogId { get; set; }
    public Blog Blog { get; set; } = null!;
}
