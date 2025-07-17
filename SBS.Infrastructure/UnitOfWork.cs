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
    internal class UnitOfWork : IUnitOfWork
	{
		private readonly AppDbContext _appDbContext;

		public IBookingRepository Bookings { get; }
		public IBookingSlotRepository BookingSlotRepository { get; }
        public IResourceRepository Resources { get; }


		public UnitOfWork(AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;
			Bookings = new BookingRepository(_appDbContext);
			BookingSlotRepository = new BookingSlotRepository(_appDbContext);
            Resources = new ResourceRepository(_appDbContext);
		}

		public async Task BeginTransactionAsync()
		{
			await _appDbContext.Database.BeginTransactionAsync();
		}

		public async Task<int> CommitAsync()
		{
			return await _appDbContext.SaveChangesAsync();
		}

		public async Task CommitTransactionAsync()
		{
			await _appDbContext.Database.CommitTransactionAsync();
		}

		public async Task RollBackTransactionAsync()
		{
			await _appDbContext.Database.RollbackTransactionAsync();
		}
	}
}
