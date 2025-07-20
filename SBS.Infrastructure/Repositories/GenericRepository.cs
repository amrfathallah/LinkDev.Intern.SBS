using Microsoft.EntityFrameworkCore;
using SBS.Application.Interfaces.IRepositories;
using SBS.Domain.Entities;
using SBS.Infrastructure.Persistence._Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Infrastructure.Repositories
{
	internal abstract class GenericRepository<T> : IRepository<T> where T : class
	{
		private readonly AppDbContext _appDbContext;

		public GenericRepository(AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}

		public virtual async Task<List<T>> GetAllAsync()
		{
			return await _appDbContext.Set<T>().ToListAsync();
		}

		public virtual Task DeleteAsync(T instance)
		{
			_appDbContext.Set<T>().Remove(instance);
			return Task.CompletedTask;
		}

		public async Task AddAsync(T instance)
		{
			await _appDbContext.Set<T>().AddAsync(instance);
		}

		public async Task<T?> GetAsync(Guid id)
		{
			return await _appDbContext.Set<T>().FindAsync(id);
		}




	}
}
