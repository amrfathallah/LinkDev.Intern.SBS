using SBS.Application.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Application.Interfaces
{
    public interface IUnitOfWork
	{
		IBookingRepository Bookings { get; }
		IBookingSlotRepository BookingSlotRepository { get; }
        IResourceRepository Resources { get; }

		Task<int> CommitAsync();
		Task BeginTransactionAsync();
		Task CommitTransactionAsync();
		Task RollBackTransactionAsync();
	}
}
