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
	public class BookingService : IBookingService
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

			//Input Validation
			if(_unitOfWork.Resources.GetAsync(requestDto.ResourceId) == null)
			{
				throw new Exception("Resource doesn't exist");
			}

			var slots = await _unitOfWork.SlotRepository.GetByIdsAsync(requestDto.SlotsIds);
			if( slots.Count != requestDto.SlotsIds.Count)
			{
				throw new Exception("Invalid slots are selected");
			}

			if(requestDto.Date < DateOnly.FromDateTime(DateTime.Today))
			{
				throw new Exception("Can't book a resource in the past");
			}


			//Check for booking conflicts
			if (await _conflictValidator.HasConflictAsync(requestDto.ResourceId, requestDto.Date, requestDto.SlotsIds))
			{
				return false;
			}
			

			//Booking Logic
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
