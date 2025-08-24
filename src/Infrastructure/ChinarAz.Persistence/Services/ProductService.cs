using ChinarAz.Application.Abstracts.Repositories;
using ChinarAz.Application.Abstracts.Services;
using ChinarAz.Application.DTOs.ProductDtos;
using ChinarAz.Application.Shared;
using ChinarAz.Domain.Entities;
using System.Net;

namespace ChinarAz.Persistence.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    // Admin
    public async Task<BaseResponse<string>> CreateAsync(ProductCreateDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            CategoryId = dto.CategoryId,
            IsWeighted = dto.IsWeighted,
            Price = dto.Price
        };

        await _productRepository.AddAsync(product);
        await _productRepository.SaveChangeAsync();

        return new BaseResponse<string>("Product created successfully", HttpStatusCode.Created);
    }

    public async Task<BaseResponse<string>> UpdateAsync(ProductUpdateDto dto)
    {
        var product = await _productRepository.GetByIdAsync(dto.Id);
        if (product == null)
            return new BaseResponse<string>("Product not found", HttpStatusCode.NotFound);

        product.Name = dto.Name;
        product.CategoryId = dto.CategoryId;
        product.IsWeighted = dto.IsWeighted;
        product.Price = dto.Price;

        _productRepository.Update(product);
        await _productRepository.SaveChangeAsync();

        return new BaseResponse<string>("Product updated successfully", HttpStatusCode.OK);
    }

    public async Task<BaseResponse<string>> DeleteAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            return new BaseResponse<string>("Product not found", HttpStatusCode.NotFound);

        _productRepository.Delete(product);
        await _productRepository.SaveChangeAsync();

        return new BaseResponse<string>("Product deleted successfully", HttpStatusCode.OK);
    }

    // Müştəri
    public async Task<BaseResponse<ProductGetDto>> GetByIdAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            return new BaseResponse<ProductGetDto>("Product not found", HttpStatusCode.NotFound);

        var dto = new ProductGetDto
        {
            Id = product.Id,
            Name = product.Name,
            CategoryId = product.CategoryId,
            IsWeighted = product.IsWeighted,
            Price = product.Price
        };

        return new BaseResponse<ProductGetDto>("Product retrieved", dto, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<List<ProductGetDto>>> GetByCategoryIdAsync(Guid categoryId)
    {
        var products = await _productRepository.GetByCategoryIdAsync(categoryId);
        var dtoList = products.Select(p => new ProductGetDto
        {
            Id = p.Id,
            Name = p.Name,
            CategoryId = p.CategoryId,
            IsWeighted = p.IsWeighted,
            Price = p.Price
        }).ToList();

        return new BaseResponse<List<ProductGetDto>>("Products retrieved", dtoList, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<List<ProductGetDto>>> SearchAsync(string keyword)
    {
        var products = await _productRepository.SearchAsync(keyword);
        var dtoList = products.Select(p => new ProductGetDto
        {
            Id = p.Id,
            Name = p.Name,
            CategoryId = p.CategoryId,
            IsWeighted = p.IsWeighted,
            Price = p.Price
        }).ToList();

        return new BaseResponse<List<ProductGetDto>>("Products retrieved", dtoList, HttpStatusCode.OK);
    }
}
