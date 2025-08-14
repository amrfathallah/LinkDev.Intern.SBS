using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SBS.Application.Interfaces;
using SBS.Domain.Entities;
using SBS.Domain.Enums;
using SBS.Infrastructure.Persistence._Data;


namespace SBS.Infrastructure
{
	internal class BookingConflictValidator : IBookingConflictValidator
	{
		private readonly AppDbContext _appDbContext;

		public BookingConflictValidator(AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}

		public async Task<bool> HasConflictAsync(Guid ResourceId, DateOnly date, List<Slot> slotIds)
		{
			//To be checked
			var existingSlots = await _appDbContext.BookingSlots.Include(slot => slot.Booking).Where(slot => slot.Booking != null && slot.Booking.ResourceId == ResourceId && slot.Booking.Date == date && slot.Booking.StatusId != (int)BookingStatusEnum.Finished).ToListAsync();

			var bookedSlots = slotIds;

			foreach (var slot in bookedSlots)
			{
				//Assuming there are no update operations
				if (existingSlots.Any(existing => existing.Slot != null && existing.Slot.StartTime < slot.EndTime && existing.Slot.EndTime > slot.StartTime))
				{
					return true;
				}
			}
			return false;
		}
	}
}
