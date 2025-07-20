using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SBS.Application.Interfaces;


using SBS.Application.Interfaces.IRepositories;
using SBS.Domain.Entities;
using SBS.Infrastructure.Persistence._Data;

namespace SBS.Infrastructure.Repositories
{
	internal class SlotRepository : GenericRepository<Slot>, ISlotRepository
	{
		private readonly AppDbContext _appDbContext;

		public SlotRepository(AppDbContext appDbContext) : base(appDbContext)
		{
			_appDbContext = appDbContext;
		}

		public Task<List<Slot>> GetByIdsAsync(List<int> slotIds)
		{
			return _appDbContext.Slots.Where(s => slotIds.Contains(s.Id)).ToListAsync();
		}
	}
}
