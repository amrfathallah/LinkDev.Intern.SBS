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

		public async Task<Resource> GetResourceWithBookedSlotsAsync(Guid resourceId, DateOnly date)
		{
			return await _appDbContext.Resources
				.Include(r => r.Bookings.Where(b => b.Date == date))
					.ThenInclude(b => b.BookingSlots)
				.FirstOrDefaultAsync(r => r.Id == resourceId);
		}
	}
} 