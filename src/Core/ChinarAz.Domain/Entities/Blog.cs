namespace ChinarAz.Domain.Entities;

public class Blog : BaseEntity
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string Author { get; set; } = null!;

    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public ICollection<Image> Images { get; set; } = new List<Image>();
}
