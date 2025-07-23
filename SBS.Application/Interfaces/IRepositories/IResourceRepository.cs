using SBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SBS.Application.Interfaces.IRepositories
{
    public interface IResourceRepository : IRepository<Resource>
    {
        Task<Resource> GetResourceWithBookedSlotsAsync(Guid resourceId, DateOnly date);
    }
} 