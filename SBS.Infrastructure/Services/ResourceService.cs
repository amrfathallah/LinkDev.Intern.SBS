using AutoMapper;
using SBS.Application.DTOs.Auth;
using SBS.Application.DTOs.ResourceDto;
using SBS.Application.Interfaces;
using SBS.Application.Interfaces.IServices;
using SBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using SBS.Application.DTOs.Common;



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
            var filtered = resources.Where(r => !r.IsDeleted).ToList();
            return _mapper.Map<List<ResourceDto>>(filtered);
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

        public async Task<bool> DeleteAsync(Guid id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var resource = await _unitOfWork.Resources.GetByIdAsync(id);
                if (resource == null) return false;
                var hasBookings = await _unitOfWork.Bookings.HasBookingsForResourceAsync(id);
                if (hasBookings) return false;
                resource.IsDeleted = true;
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

        public async Task<ResourceBookedSlotsDto> GetBookedSlotsAsync(GetBookedSlotsRequestDto request)
        {
            var dateOnly = DateOnly.FromDateTime(request.Date);
            var resource = await _unitOfWork.Resources.GetResourceWithBookedSlotsAsync(request.ResourceId, dateOnly);
            if (resource == null)
                throw new InvalidOperationException("Resource not found");

            var bookedSlots = resource.Bookings
                .SelectMany(b => b.BookingSlots)
                .Where(bs => bs.Slot != null)
                .Select(bs => new BookedSlotDto
                {
                    SlotId = bs.SlotId,
                    StartTime = bs.Slot!.StartTime,
                    EndTime = bs.Slot!.EndTime
                })
                .ToList();

            return new ResourceBookedSlotsDto
            {
                ResourceId = resource.Id,
                ResourceName = resource.Name,
                BookedSlots = bookedSlots
            };
        }
		public async Task<ApiResponse<List<ResourceTypeDto>>> GetAllResourceTypesAsync()
		{
			try
			{
				var types = await _unitOfWork.ResourceType.GetAllAsync();
				var result = _mapper.Map<List<ResourceTypeDto>>(types);
				
				return new ApiResponse<List<ResourceTypeDto>>
				{
					Success = true,
					Message = "Resource types retrieved successfully",
					Data = result
				};
			}
			catch (Exception)
			{
				return new ApiResponse<List<ResourceTypeDto>>
				{
					Success = false,
					Message = "An error occurred while fetching resource types",
					Data = null
				};
			}
		}

		
	}
        }

        public async Task<List<ResourceDto>> GetAvailableResourcesAsync(DateOnly date)
        {
            var resources = await _unitOfWork.Resources.GetAllAsync();
            var filteredResources = resources.Where(r => r.IsActive && !r.IsDeleted).ToList();

            var allSlots = await _unitOfWork.SlotRepository.GetAllAsync();
            var availableResources = new List<ResourceDto>();

            foreach (var resource in filteredResources)
            {
                var resourceWithBookings = await _unitOfWork.Resources.GetResourceWithBookedSlotsAsync(resource.Id, date);
                var bookedSlotIds = resourceWithBookings?.Bookings
                    .SelectMany(b => b.BookingSlots)
                    .Where(bs => bs.Slot != null)
                    .Select(bs => bs.SlotId)
                    .ToHashSet() ?? new HashSet<int>();

                if (allSlots.Any(slot => !bookedSlotIds.Contains(slot.Id)))
                {
                    availableResources.Add(_mapper.Map<ResourceDto>(resource));
                }
            }

            return availableResources;
        }
    }
}