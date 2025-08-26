namespace ChinarAz.Application.DTOs.BioDtos;

public record class BioCreateDto
{
    public string Key { get; set; } = null!;
    public string Value { get; set; } = null!;
}