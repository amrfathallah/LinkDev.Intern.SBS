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
using Microsoft.TeamFoundation;

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
            if (await _unitOfWork.Resources.GetByIdAsync(requestDto.ResourceId) == null)
            {
                throw new Exception("Resource doesn't exist");
            }

            var slots = await _unitOfWork.SlotRepository.GetByIdsAsync(requestDto.SlotsIds);
            if (slots.Count != requestDto.SlotsIds.Count)
            {
                throw new Exception("Invalid slots are selected");
            }


            if(slots.Any(slot => slot.StartTime < DateTime.UtcNow.TimeOfDay) || requestDto.Date < DateOnly.FromDateTime(DateTime.UtcNow))
            {
                throw new Exception("Selected slots are in the past");
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

        public async Task<bool> CancelBookingAsync(Guid bookingId, Guid userId)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingId);
            if (booking == null)
            {
                throw new Exception("Booking not found");
            }
            if (booking.UserId != userId)
            {
                throw new Exception("You can only cancel your own bookings");
            }
            var timeRange = GetSlotTimeRange(booking.BookingSlots);
            if (timeRange.StartTime.Subtract(DateTime.Now.TimeOfDay).TotalMinutes < 30 && booking.Date == DateOnly.FromDateTime(DateTime.Now))
            {
                throw new Exception("Cancellation is not allowed within 30 minutes before start time");
            }
            var result = await _unitOfWork.Bookings.CancelBookingAsync(bookingId);
            await _unitOfWork.CommitAsync();
            return result;
        }

        public async Task<List<MyBookingDto>> GetBookingsByUserAsync(Guid userId)
        {
            var bookings = await _unitOfWork.Bookings.GetBookingsByUserAsync(userId);
            return bookings.Select(booking => new MyBookingDto(
                booking.Id,
                booking.Resource.Name,
                booking.Date,
                GetBookingStatus(booking),
                GetSlotTimeRange(booking.BookingSlots).StartTime,
                GetSlotTimeRange(booking.BookingSlots).EndTime
            )).ToList();
        }
        private BookingStatusEnum GetBookingStatus(Booking booking)
        {
            var TimeRange = GetSlotTimeRange(booking.BookingSlots);
            if (booking.Date.Day.Equals(DateTime.Today.Day))
            {
                var currentTime = DateTime.Now.TimeOfDay;
                BookingStatusEnum status;
                if (currentTime < TimeRange.StartTime)
                {
                    status = BookingStatusEnum.Upcoming;
                }
                else if (currentTime >= TimeRange.StartTime && currentTime <= TimeRange.EndTime)
                {
                    status = BookingStatusEnum.Happening;
                }
                else
                {
                    status = BookingStatusEnum.Finished;
                }
                return status;
            }
            return DateOnly.FromDateTime(DateTime.Now) > booking.Date? BookingStatusEnum.Finished : BookingStatusEnum.Upcoming;
        }
        private (TimeSpan StartTime, TimeSpan EndTime) GetSlotTimeRange(IEnumerable<BookingSlot> bookingSlots)
        {
            var startTime = bookingSlots.Min(bs => bs.Slot.StartTime);
            var endTime = bookingSlots.Max(bs => bs.Slot.EndTime);
            return (startTime, endTime);
        }
    }
}