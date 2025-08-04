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

		public async Task<bool> CancelBookingAsync(Guid bookingId)
		{
			var booking = await _appDbContext.Bookings.FindAsync(bookingId);
			if (booking == null)
			{
				throw new Exception("Booking not found");
			}
			if (booking.IsDeleted)
			{
				throw new Exception("Booking is already deleted");
			}
			booking.IsDeleted = true;
			return true;
		}

		public async Task<IEnumerable<Booking>> GetBookingsByUserAsync(Guid userId)
		{
			return await _appDbContext.Bookings
				.Include(b => b.BookingSlots)
					.ThenInclude(bs => bs.Slot)
				.Include(b => b.Resource)
				.Where(b => b.UserId == userId)
				.Where(b => !b.IsDeleted)
				.ToListAsync();
		}

		public async Task<bool> HasBookingsForResourceAsync(Guid resourceId)
		{
			return await _appDbContext.Bookings.AnyAsync(b => b.ResourceId == resourceId);
		}
		public IQueryable<Booking> GetAllBookingWithIncludes()
		{
			return _appDbContext.Bookings
				.Include(b => b.User)
				.Include(b => b.Status)
				.Include(b => b.Resource).ThenInclude(r => r!.Type)
				.Include(b => b.BookingSlots).ThenInclude(b => b.Slot);
		}

		
	}
}
