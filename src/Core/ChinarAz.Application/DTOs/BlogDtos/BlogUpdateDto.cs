using Microsoft.AspNetCore.Http;

namespace ChinarAz.Application.DTOs.BlogDtos;

public record class BlogUpdateDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string Author { get; set; } = null!;
    public Guid CategoryId { get; set; }
    public List<IFormFile>? Images { get; set; } // Yeni şəkillər
}

