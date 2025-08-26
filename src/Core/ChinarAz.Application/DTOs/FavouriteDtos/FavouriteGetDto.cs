namespace ChinarAz.Application.DTOs.FavouriteDtos;

public record class FavouriteGetDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
}
