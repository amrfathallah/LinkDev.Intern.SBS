using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SBS.Domain.Entities;
using SBS.Application.DTOs.BookingDto;

namespace SBS.Application.Interfaces.IServices
{
    public interface IBookingService
    {
        public Task<bool> BookAsync(BookingRequestDto requestDto, Guid userId, string createdBy);
        public Task<List<MyBookingDto>> GetBookingsByUserAsync(Guid userId);
        public Task<bool> CancelBookingAsync(Guid bookingId, Guid userId);
    }
}
