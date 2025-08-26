namespace ChinarAz.Application.DTOs.BioDtos;

public record class BioUpdateDto
{
    public Guid Id { get; set; }
    public string Key { get; set; } = null!;
    public string Value { get; set; } = null!;
}
