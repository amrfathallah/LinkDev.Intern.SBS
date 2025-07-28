using Microsoft.EntityFrameworkCore;
using SBS.Application.Interfaces.IRepositories;
using SBS.Application.Interfaces.IServices;
using SBS.Domain.Entities;
using SBS.Infrastructure.Persistence._Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Infrastructure.Repositories
{
    internal class BookingRepository : GenericRepository<Booking>, IBookingRepository
	{
		private readonly AppDbContext _appDbContext;

		public BookingRepository(AppDbContext appDbContext) : base(appDbContext)
		{
			_appDbContext = appDbContext;
		}

		

		public async Task<bool> HasBookingsForResourceAsync(Guid resourceId)
		{
			return await _appDbContext.Bookings.AnyAsync(b => b.ResourceId == resourceId);
		}
	}
}
