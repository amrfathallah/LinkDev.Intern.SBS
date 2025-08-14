using Microsoft.EntityFrameworkCore;
using SBS.Application.Interfaces.IRepositories;
using SBS.Domain.Entities;
using SBS.Infrastructure.Persistence._Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Infrastructure.Repositories
{
    internal class ResourceRepository : GenericRepository<Resource>, IResourceRepository
    {
        private readonly AppDbContext _appDbContext;

        public ResourceRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Resource?> GetByIdAsync(Guid id)
        {
            return await _appDbContext.Resources.Include(r => r.Type).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task AddAsync(Resource resource)
        {
            await _appDbContext.Resources.AddAsync(resource);
        }
        public async Task UpdateAsync(Resource resource)
        {
            _appDbContext.Resources.Update(resource);
        }

        public async Task<Resource> GetResourceWithBookedSlotsAsync(Guid resourceId, DateOnly date)
        {
            return await _appDbContext.Resources
                .Include(r => r.Bookings.Where(b => b.Date == date))
                    .ThenInclude(b => b.BookingSlots)
                        .ThenInclude(bs => bs.Slot) 
                .FirstOrDefaultAsync(r => r.Id == resourceId);
        }

        public async Task<List<Resource>> GetResourcesWithAvailableSlotsAsync(DateOnly date)
        {
            var allSlots = await _appDbContext.Slots.ToListAsync();

            var resources = await _appDbContext.Resources
                .Where(r => r.IsActive && !r.IsDeleted)
                .Include(r => r.Bookings.Where(b => b.Date == date))
                    .ThenInclude(b => b.BookingSlots)
                        .ThenInclude(bs => bs.Slot)
                .ToListAsync();

            var availableResources = new List<Resource>();

            foreach (var resource in resources)
            {
                var bookedSlotIds = resource.Bookings
                    .SelectMany(b => b.BookingSlots)
                    .Where(bs => bs.Slot != null)
                    .Select(bs => bs.SlotId)
                    .ToHashSet();

                if (allSlots.Any(slot => !bookedSlotIds.Contains(slot.Id)))
                {
                    availableResources.Add(resource);
                }
            }

            return availableResources;
        }

    }
} 