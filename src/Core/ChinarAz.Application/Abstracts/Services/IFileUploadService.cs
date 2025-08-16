using Microsoft.AspNetCore.Http;

namespace ChinarAz.Application.Abstracts.Services;

public interface IFileUploadService
{
    Task<string> UploadAsync(IFormFile file);
}
