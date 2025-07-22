using SBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Application.Interfaces.IRepositories
{
	public interface IRepository<T> where T : class
	{
        Task<T?> GetByIdAsync(Guid id);
        Task AddAsync(T instance);
        Task<List<T>> GetAllAsync();
		Task UpdateAsync(T instance);
	}
}
