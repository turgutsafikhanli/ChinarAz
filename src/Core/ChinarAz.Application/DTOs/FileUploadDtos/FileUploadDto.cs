using Microsoft.AspNetCore.Http;

namespace ChinarAz.Application.DTOs.FileUploadDtos;

public record class FileUploadDto
{
    public IFormFile File { get; set; } = null!;
}
