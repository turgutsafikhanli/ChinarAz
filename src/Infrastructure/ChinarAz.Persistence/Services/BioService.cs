using ChinarAz.Application.Abstracts.Repositories;
using ChinarAz.Application.Abstracts.Services;
using ChinarAz.Application.DTOs.BioDtos;
using ChinarAz.Application.Shared;
using ChinarAz.Domain.Entities;
using System.Net;

namespace ChinarAz.Persistence.Services;

public class BioService : IBioService
{
    private readonly IBioRepository _bioRepository;

    public BioService(IBioRepository bioRepository)
    {
        _bioRepository = bioRepository;
    }

    public async Task<BaseResponse<string>> CreateAsync(BioCreateDto dto)
    {
        var bio = new Bio
        {
            Id = Guid.NewGuid(),
            Key = dto.Key,
            Value = dto.Value
        };

        await _bioRepository.AddAsync(bio);
        await _bioRepository.SaveChangeAsync();

        return new BaseResponse<string>("Bio created successfully", bio.Id.ToString(), HttpStatusCode.Created);
    }

    public async Task<BaseResponse<string>> UpdateAsync(BioUpdateDto dto)
    {
        var bio = await _bioRepository.GetByIdAsync(dto.Id);
        if (bio == null)
            return new BaseResponse<string>("Bio not found", false, HttpStatusCode.NotFound);

        bio.Key = dto.Key;
        bio.Value = dto.Value;

        _bioRepository.Update(bio);
        await _bioRepository.SaveChangeAsync();

        return new BaseResponse<string>("Bio updated successfully", bio.Id.ToString(), HttpStatusCode.OK);
    }

    public async Task<BaseResponse<string>> DeleteAsync(Guid id)
    {
        var bio = await _bioRepository.GetByIdAsync(id);
        if (bio == null)
            return new BaseResponse<string>("Bio not found", false, HttpStatusCode.NotFound);

        _bioRepository.Delete(bio);
        await _bioRepository.SaveChangeAsync();

        return new BaseResponse<string>("Bio deleted successfully", HttpStatusCode.OK);
    }

    public async Task<BaseResponse<BioGetDto>> GetByIdAsync(Guid id)
    {
        var bio = await _bioRepository.GetByIdAsync(id);
        if (bio == null)
            return new BaseResponse<BioGetDto>("Bio not found", false, HttpStatusCode.NotFound);

        var dto = new BioGetDto
        {
            Id = bio.Id,
            Key = bio.Key,
            Value = bio.Value
        };

        return new BaseResponse<BioGetDto>("Success", dto, HttpStatusCode.OK);
    }

    public async Task<BaseResponse<List<BioGetDto>>> GetAllAsync()
    {
        var bios = _bioRepository.GetAll().ToList();

        var dtos = bios.Select(b => new BioGetDto
        {
            Id = b.Id,
            Key = b.Key,
            Value = b.Value
        }).ToList();

        return new BaseResponse<List<BioGetDto>>("Success", dtos, HttpStatusCode.OK);
    }
}
