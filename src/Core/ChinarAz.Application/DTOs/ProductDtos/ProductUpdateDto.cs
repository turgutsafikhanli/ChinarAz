using Microsoft.AspNetCore.Http;

namespace ChinarAz.Application.DTOs.ProductDtos;

public record class ProductUpdateDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid CategoryId { get; set; }
    public bool IsWeighted { get; set; }
    public decimal Price { get; set; }
    public List<IFormFile>? Images { get; set; } = new();
    public List<string>? ImageUrls { get; set; } = new();
}
