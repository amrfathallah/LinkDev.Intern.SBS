using SBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Application.Interfaces.IRepositories
{
	public interface IBookingSlotRepository : IRepository<BookingSlot>
	{
		Task AddRangeAsync(List<BookingSlot> bookingSlot);
		Task<List<BookingSlot>?> GetRangeAsync(Guid id);

	}
}
