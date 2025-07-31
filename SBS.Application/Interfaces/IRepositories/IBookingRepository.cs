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
        Task<bool> HasBookingsForResourceAsync(Guid resourceId);

         IQueryable<Booking> GetAllBookingWithIncludes();
    }
}
