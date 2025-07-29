using SBS.Application.Interfaces;
using SBS.Infrastructure.Persistence._Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SBS.Infrastructure.Repositories;
using SBS.Application.Interfaces.IRepositories;

namespace SBS.Infrastructure
{
    internal class UnitOfWork(AppDbContext appDbContext) : IUnitOfWork
	{
		private IBookingRepository? _bookings;
		private IBookingSlotRepository? _bookingSlots;
		private ISlotRepository? _slotRepository;
		private IResourceRepository? _resourceRepository;


		public IBookingRepository Bookings => _bookings ??= new BookingRepository(appDbContext);
		public IBookingSlotRepository BookingSlotRepository => _bookingSlots ??= new BookingSlotRepository(appDbContext);

		public IResourceRepository Resources => _resourceRepository ??= new ResourceRepository(appDbContext);

		public ISlotRepository SlotRepository => _slotRepository ??= new SlotRepository(appDbContext);

		public async Task BeginTransactionAsync()
		{
			await appDbContext.Database.BeginTransactionAsync();
		}

		public async Task<int> CommitAsync()
		{
			return await appDbContext.SaveChangesAsync();
		}

		public async Task CommitTransactionAsync()
		{
			await appDbContext.Database.CommitTransactionAsync();
		}

		public async Task RollBackTransactionAsync()
		{
			await appDbContext.Database.RollbackTransactionAsync();
		}
	}
}
