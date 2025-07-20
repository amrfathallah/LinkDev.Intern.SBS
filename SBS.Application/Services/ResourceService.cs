using AutoMapper;
using SBS.Application.DTOs.ResourceDto;
using SBS.Application.Interfaces;
using SBS.Application.Interfaces.IServices;
using SBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SBS.Application.Services
{
    public class ResourceService : IResourceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ResourceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ResourceDto>> GetAllAsync()
        {
            var resources = await _unitOfWork.Resources.GetAllAsync();
            return _mapper.Map<List<ResourceDto>>(resources);
        }

        public async Task<ResourceDto?> GetByIdAsync(Guid id)
        {
            var resource = await _unitOfWork.Resources.GetByIdAsync(id);
            return resource == null ? null : _mapper.Map<ResourceDto>(resource);
        }

        public async Task<ResourceDto> AddAsync(CreateResourceDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var resource = _mapper.Map<Resource>(dto);
                resource.IsActive = true;
                await _unitOfWork.Resources.AddAsync(resource);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return _mapper.Map<ResourceDto>(resource);
            }
            catch
            {
                await _unitOfWork.RollBackTransactionAsync();
                throw;
            }
        }

        public async Task<ResourceDto?> UpdateAsync(Guid id, UpdateResourceDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var resource = await _unitOfWork.Resources.GetByIdAsync(id);
                if (resource is null) return null;
                resource.Name = dto.Name;
                resource.Capacity = dto.Capacity;
                resource.OpenAt = dto.OpenAt;
                resource.CloseAt = dto.CloseAt;
                resource.IsActive = dto.IsActive;
                await _unitOfWork.Resources.UpdateAsync(resource);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return _mapper.Map<ResourceDto>(resource);
            }
            catch
            {
                await _unitOfWork.RollBackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> SetActiveStatusAsync(Guid id, bool isActive)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var resource = await _unitOfWork.Resources.GetByIdAsync(id);
                if (resource == null) return false;
                resource.IsActive = isActive;
                await _unitOfWork.Resources.UpdateAsync(resource);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch
            {
                await _unitOfWork.RollBackTransactionAsync();
                throw;
            }
        }
    }
} 