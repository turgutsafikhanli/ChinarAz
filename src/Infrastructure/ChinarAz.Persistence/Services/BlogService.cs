using ChinarAz.Application.Abstracts.Repositories;
using ChinarAz.Application.Abstracts.Services;
using ChinarAz.Application.DTOs.BlogDtos;
using ChinarAz.Application.Shared;
using ChinarAz.Domain.Entities;
using System.Net;

namespace ChinarAz.Persistence.Services;

public class BlogService : IBlogService
{
    private readonly IBlogRepository _blogRepository;
    private readonly IFileUploadService _fileUploadService;

    public BlogService(IBlogRepository blogRepository, IFileUploadService fileUploadService)
    {
        _blogRepository = blogRepository;
        _fileUploadService = fileUploadService;
    }

    public async Task<BaseResponse<string>> CreateAsync(BlogCreateDto dto)
    {
        try
        {
            var imageUrls = new List<string>();
            if (dto.Images != null && dto.Images.Any())
            {
                foreach (var file in dto.Images)
                {
                    var url = await _fileUploadService.UploadAsync(file);
                    imageUrls.Add(url);
                }
            }

            var blog = new Blog
            {
                Title = dto.Title,
                Content = dto.Content,
                Author = dto.Author,
                CategoryId = dto.CategoryId,
                Images = imageUrls.Select(url => new Image { ImageUrl = url }).ToList()
            };

            await _blogRepository.AddAsync(blog);
            await _blogRepository.SaveChangeAsync();

            return new BaseResponse<string>("Blog created successfully", blog.Id.ToString(), HttpStatusCode.Created);
        }
        catch (Exception ex)
        {
            return new BaseResponse<string>($"Error creating blog: {ex.Message}", false, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<BaseResponse<string>> UpdateAsync(BlogUpdateDto dto)
    {
        try
        {
            var blog = await _blogRepository.GetWithImagesByIdAsync(dto.Id);
            if (blog == null)
                return new BaseResponse<string>("Blog not found", false, HttpStatusCode.NotFound);

            blog.Title = dto.Title;
            blog.Content = dto.Content;
            blog.Author = dto.Author;
            blog.CategoryId = dto.CategoryId;

            if (dto.Images != null && dto.Images.Any())
            {
                // Köhnə şəkilləri sil və yeni əlavə et
                blog.Images.Clear();
                foreach (var file in dto.Images)
                {
                    var url = await _fileUploadService.UploadAsync(file);
                    blog.Images.Add(new Image { ImageUrl = url });
                }
            }

            _blogRepository.Update(blog);
            await _blogRepository.SaveChangeAsync();

            return new BaseResponse<string>("Blog updated successfully", blog.Id.ToString(), HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return new BaseResponse<string>($"Error updating blog: {ex.Message}", false, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<BaseResponse<string>> DeleteAsync(Guid id)
    {
        try
        {
            var blog = await _blogRepository.GetWithImagesByIdAsync(id);
            if (blog == null)
                return new BaseResponse<string>("Blog not found", false, HttpStatusCode.NotFound);

            await _blogRepository.SoftDeleteAsync(blog);

            return new BaseResponse<string>("Blog deleted successfully", HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return new BaseResponse<string>($"Error deleting blog: {ex.Message}", false, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<BaseResponse<BlogGetDto>> GetByIdAsync(Guid id)
    {
        var blog = await _blogRepository.GetWithImagesByIdAsync(id);
        if (blog == null)
            return new BaseResponse<BlogGetDto>("Blog not found", false, HttpStatusCode.NotFound);

        var dto = new BlogGetDto
        {
            Id = blog.Id,
            Title = blog.Title,
            Content = blog.Content,
            Author = blog.Author,
            CategoryId = blog.CategoryId,
            ImageUrls = blog.Images.Select(i => i.ImageUrl).ToList()
        };

        return new BaseResponse<BlogGetDto>("Blog retrieved successfully", dto, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<List<BlogGetDto>>> GetAllAsync()
    {
        var blogs = await _blogRepository.GetAllWithImagesAsync();

        var dtoList = blogs.Select(blog => new BlogGetDto
        {
            Id = blog.Id,
            Title = blog.Title,
            Content = blog.Content,
            Author = blog.Author,
            CategoryId = blog.CategoryId,
            ImageUrls = blog.Images.Select(i => i.ImageUrl).ToList()
        }).ToList();

        return new BaseResponse<List<BlogGetDto>>("Blogs retrieved successfully", dtoList, HttpStatusCode.OK);
    }
}
