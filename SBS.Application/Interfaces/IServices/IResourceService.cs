using SBS.Application.DTOs.ResourceDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SBS.Application.Interfaces.IServices
{
    public interface IResourceService
    {
        Task<List<ResourceDto>> GetAllAsync();
        Task<ResourceDto?> GetByIdAsync(Guid id);
        Task<ResourceDto> AddAsync(CreateResourceDto dto);
        Task<ResourceDto?> UpdateAsync(Guid id, UpdateResourceDto dto);
        Task<bool> SetActiveStatusAsync(Guid id, bool isActive);
        Task<bool> DeleteAsync(Guid id);
        Task<ResourceBookedSlotsDto> GetBookedSlotsAsync(GetBookedSlotsRequestDto request);
        Task<List<ResourceDto>> GetAvailableResourcesAsync(DateOnly date);
    }
} 