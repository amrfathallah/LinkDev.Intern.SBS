using SBS.Application.Interfaces;
using SBS.Application.Interfaces.IServices;
using SBS.Domain.Entities;
using SBS.Domain.Enums;
using SBS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Application.Services
{
	internal class BookingService : IBookingService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IBookingConflictValidator _conflictValidator;

		public BookingService(IUnitOfWork unitOfWork, IBookingConflictValidator bookingConflictValidator)
		{
			_unitOfWork = unitOfWork;
			_conflictValidator = bookingConflictValidator;
		}

		public async Task<bool> BookAsync(BookingRequestDto requestDto, Guid userId, string createdBy)
		{
			await _unitOfWork.BeginTransactionAsync();

			if (await _conflictValidator.HasConflictAsync(requestDto.ResourceId, requestDto.Date, requestDto.SlotsIds))
			{
				return false;
			}

			var booking = new Booking
			{
				UserId = userId,
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
