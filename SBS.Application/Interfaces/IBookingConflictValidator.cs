using SBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Application.Interfaces
{
	public interface IBookingConflictValidator
	{
		Task<bool> HasConflictAsync(Guid ResourceId, DateOnly date, List<Slot> Slots);
	}
}