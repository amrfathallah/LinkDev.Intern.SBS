using Microsoft.EntityFrameworkCore;
using SBS.Application.Interfaces.IRepositories;
using SBS.Domain.Entities;
using SBS.Infrastructure.Persistence._Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SBS.Infrastructure.Repositories
{
    internal class ResourceRepository : GenericRepository<Resource>, IResourceRepository
    {
        private readonly AppDbContext _appDbContext;

        public ResourceRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Resource?> GetByIdAsync(Guid id)
        {
            return await _appDbContext.Resources.Include(r => r.Type).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task AddAsync(Resource resource)
        {
            await _appDbContext.Resources.AddAsync(resource);
        }
    }
} 