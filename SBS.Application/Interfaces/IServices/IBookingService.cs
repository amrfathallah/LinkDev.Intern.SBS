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

		public Task<Pagination<ViewAllBookingDto>> GetAllBookingsAsync( ViewBookingsParams viewBookingsParams );

		Task<List<BookingStatusDto>> GetAllBookingStatusAsync();

		Task<List<BookingsUsersDto>> GetUsersWithBookingsAsync();

	}
}
