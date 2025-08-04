using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SBS.Domain.Entities;
using SBS.Application.DTOs.BookingDto;
using SBS.Application.DTOs.Common;

namespace SBS.Application.Interfaces.IServices
{
    public interface IBookingService
    {
        public Task<bool> BookAsync(BookingRequestDto requestDto, Guid userId, string createdBy);
        public Task<List<MyBookingDto>> GetBookingsByUserAsync(Guid userId);
        public Task<bool> CancelBookingAsync(Guid bookingId, Guid userId);

		public Task<Pagination<ViewAllBookingDto>> GetAllBookingsAsync( ViewBookingsParams viewBookingsParams );

		Task<List<BookingStatusDto>> GetAllBookingStatusAsync();

	}
}
