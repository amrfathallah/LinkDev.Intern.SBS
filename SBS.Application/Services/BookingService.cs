using SBS.Application.Interfaces;
using SBS.Application.Interfaces.IServices;
using SBS.Domain.Entities;
using SBS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SBS.Application.DTOs.BookingDto;

namespace SBS.Application.Services
{
	internal class BookingService : IBookingService
	{
		private readonly IUnitOfWork _unitOfWork;

		public BookingService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<bool> BookAsync(BookingRequestDto requestDto, Guid userId, string createdBy)
		{
			await _unitOfWork.BeginTransactionAsync();

			//Make Conflict Logic

			var booking = new Booking
			{
				UserId = userId, //Check
				ResourceId = requestDto.ResourceId,
				Date = requestDto.Date,
				StatusId = (int)BookingStatusEnum.Upcoming,
				CreatedAt = DateTime.UtcNow,
				CreatedBy = createdBy,

			};

			await _unitOfWork.Bookings.AddAsync(booking);
			await _unitOfWork.CommitAsync();

			var bookingSlots = requestDto.SlotsIds.Select(slotId => new BookingSlot
			{
				SlotId = slotId,
				BookingId = booking.Id,
			}).ToList();

			await _unitOfWork.BookingSlotRepository.AddRangeAsync(bookingSlots);
			await _unitOfWork.CommitAsync();
			
			await _unitOfWork.CommitTransactionAsync();

			return true;




		}
	}
}
