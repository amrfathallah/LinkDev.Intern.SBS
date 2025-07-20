using SBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Application.Interfaces.IRepositories
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task AddAsync(Booking booking);
		Task<Booking?> GetAsync(Guid id);
        Task<bool> HasBookingsForResourceAsync(Guid resourceId);
	}
}
