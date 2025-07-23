using Microsoft.EntityFrameworkCore;
using SBS.Application.Interfaces.IRepositories;
using SBS.Domain.Entities;
using SBS.Infrastructure.Persistence._Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Infrastructure.Repositories
{
	internal class BookingSlotRepository : GenericRepository<BookingSlot>, IBookingSlotRepository
	{
		private readonly AppDbContext _appDbContext;

		public BookingSlotRepository(AppDbContext appDbContext) : base(appDbContext)
		{
			_appDbContext = appDbContext;
		}

		public async Task AddRangeAsync(List<BookingSlot> bookingSlot)
		{
			foreach(BookingSlot booking in bookingSlot)
			{
				await _appDbContext.AddAsync(booking);
			}
		}

		public async Task<List<BookingSlot>?> GetAsync(Guid id)
		{
			return await _appDbContext.BookingSlots.Include(bs => bs.Slot).Where(bs => bs.BookingId == id).ToListAsync();
		}
	}
}
