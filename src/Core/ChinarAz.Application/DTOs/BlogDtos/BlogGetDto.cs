namespace ChinarAz.Application.DTOs.BlogDtos;

public record class BlogGetDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string Author { get; set; } = null!;
    public Guid CategoryId { get; set; }
    public List<string> ImageUrls { get; set; } = new();
}
