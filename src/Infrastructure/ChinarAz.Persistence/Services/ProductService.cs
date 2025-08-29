using ChinarAz.Application.Abstracts.Repositories;
using ChinarAz.Application.Abstracts.Services;
using ChinarAz.Application.DTOs.ProductDtos;
using ChinarAz.Application.Models;
using ChinarAz.Application.Shared;
using ChinarAz.Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System.Net;

namespace ChinarAz.Persistence.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IFileUploadService _fileUploadService;
    private readonly ICacheService _cacheService;
    private readonly IElasticService _elasticService;

    private readonly TimeSpan _cacheExpiryShort = TimeSpan.FromMinutes(5);
    private readonly TimeSpan _cacheExpiryLong = TimeSpan.FromMinutes(10);

    public ProductService(
        IProductRepository productRepository,
        IFileUploadService fileUploadService,
        ICacheService cacheService,
        IElasticService elasticService)
    {
        _productRepository = productRepository;
        _fileUploadService = fileUploadService;
        _cacheService = cacheService;
        _elasticService = elasticService;
    }

    // =======================
    // Admin CRUD
    // =======================
    public async Task<BaseResponse<string>> CreateAsync(ProductCreateDto dto)
    {
        try
        {
            var uploadedImages = new List<string>();
            if (dto.Images != null && dto.Images.Any())
            {
                foreach (var file in dto.Images)
                {
                    var url = await _fileUploadService.UploadAsync(file);
                    uploadedImages.Add(url);
                }
            }

            var product = new Product
            {
                Name = dto.Name,
                CategoryId = dto.CategoryId,
                IsWeighted = dto.IsWeighted,
                Price = dto.Price,
                Images = uploadedImages.Select(u => new Image { ImageUrl = u }).ToList()
            };

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangeAsync();

            var elasticModel = new ProductElasticModel
            {
                Id = product.Id,
                Name = product.Name,
                CategoryId = product.CategoryId,
                Price = product.Price,
                IsWeighted = product.IsWeighted
            };
            await _elasticService.IndexProductAsync(elasticModel);

            await _cacheService.RemoveAsync($"products_category_{dto.CategoryId}");

            return new BaseResponse<string>("Product created successfully", product.Id.ToString(), HttpStatusCode.Created);
        }
        catch (Exception ex)
        {
            return new BaseResponse<string>($"Error creating product: {ex.Message}", false, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<BaseResponse<string>> UpdateAsync(ProductUpdateDto dto)
    {
        try
        {
            var product = await _productRepository.GetByIdAsync(dto.Id);
            if (product == null)
                return new BaseResponse<string>("Product not found", false, HttpStatusCode.NotFound);

            if (dto.Images != null && dto.Images.Any())
            {
                var uploadedImages = new List<string>();
                foreach (var file in dto.Images)
                    uploadedImages.Add(await _fileUploadService.UploadAsync(file));

                product.Images.Clear();
                product.Images = uploadedImages.Select(u => new Image { ImageUrl = u }).ToList();
            }

            product.Name = dto.Name;
            product.CategoryId = dto.CategoryId;
            product.IsWeighted = dto.IsWeighted;
            product.Price = dto.Price;

            _productRepository.Update(product);
            await _productRepository.SaveChangeAsync();

            var elasticModel = new ProductElasticModel
            {
                Id = product.Id,
                Name = product.Name,
                CategoryId = product.CategoryId,
                Price = product.Price,
                IsWeighted = product.IsWeighted
            };
            await _elasticService.UpdateProductAsync(elasticModel);

            await _cacheService.RemoveAsync($"product_{product.Id}");
            await _cacheService.RemoveAsync($"products_category_{product.CategoryId}");
            await _cacheService.RemoveAsync($"search_*");

            return new BaseResponse<string>("Product updated successfully", product.Id.ToString(), HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return new BaseResponse<string>($"Error updating product: {ex.Message}", false, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<BaseResponse<string>> DeleteAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            return new BaseResponse<string>("Product not found", false, HttpStatusCode.NotFound);

        _productRepository.Delete(product);
        await _productRepository.SaveChangeAsync();

        // Elasticsearch delete
        await _elasticService.DeleteProductAsync(id);

        // Cache invalidation
        await _cacheService.RemoveAsync($"product_{id}");
        await _cacheService.RemoveAsync($"products_category_{product.CategoryId}");
        await _cacheService.RemoveAsync($"search_*");

        return new BaseResponse<string>("Product deleted successfully", HttpStatusCode.OK);
    }

    // =======================
    // Müştəri metodları
    // =======================
    public async Task<BaseResponse<ProductGetDto>> GetByIdAsync(Guid id)
    {
        var cacheKey = $"product_{id}";
        var cached = await _cacheService.GetAsync<ProductGetDto>(cacheKey);
        if (cached != null)
            return new BaseResponse<ProductGetDto>("Product retrieved (from cache)", cached, HttpStatusCode.OK);

        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            return new BaseResponse<ProductGetDto>("Product not found", false, HttpStatusCode.NotFound);

        var dto = MapToDto(product);
        await _cacheService.SetAsync(cacheKey, dto, _cacheExpiryShort);

        return new BaseResponse<ProductGetDto>("Product retrieved", dto, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<List<ProductGetDto>>> GetByCategoryIdAsync(Guid categoryId)
    {
        var cacheKey = $"products_category_{categoryId}";
        var cached = await _cacheService.GetAsync<List<ProductGetDto>>(cacheKey);
        if (cached != null)
            return new BaseResponse<List<ProductGetDto>>("Products retrieved (from cache)", cached, HttpStatusCode.OK);

        var products = await _productRepository.GetByCategoryIdAsync(categoryId);
        var dtos = products.Select(MapToDto).ToList();

        await _cacheService.SetAsync(cacheKey, dtos, _cacheExpiryLong);
        return new BaseResponse<List<ProductGetDto>>("Products retrieved", dtos, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<List<ProductGetDto>>> SearchAsync(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return new BaseResponse<List<ProductGetDto>>(
                "Keyword is empty",
                new List<ProductGetDto>(),
                HttpStatusCode.BadRequest
            );

        var cacheKey = $"search_{keyword.ToLower()}";

        var cached = await _cacheService.GetAsync<List<ProductGetDto>>(cacheKey);
        if (cached != null && cached.Any())
        {
            return new BaseResponse<List<ProductGetDto>>(
                "Products retrieved (from cache)",
                cached,
                HttpStatusCode.OK
            );
        }

        var productsFromElastic = await _elasticService.SearchProductsAsync(keyword);

        if (productsFromElastic == null || !productsFromElastic.Any())
        {
            // Əgər nəticə tapılmadısa cache-ə yazmırıq
            return new BaseResponse<List<ProductGetDto>>(
                "No products found for your search.",
                new List<ProductGetDto>(),
                HttpStatusCode.OK
            );
        }

        var dtos = productsFromElastic
            .Where(p => p != null)
            .Select(p => new ProductGetDto
            {
                Id = p.Id,
                Name = p.Name,
                CategoryId = p.CategoryId,
                IsWeighted = p.IsWeighted,
                Price = p.Price
            })
            .ToList();

        await _cacheService.SetAsync(cacheKey, dtos, TimeSpan.FromDays(1));

        return new BaseResponse<List<ProductGetDto>>(
            "Products retrieved",
            dtos,
            HttpStatusCode.OK
        );
    }
    private ProductGetDto MapToDto(Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        CategoryId = product.CategoryId,
        IsWeighted = product.IsWeighted,
        Price = product.Price
    };
}
