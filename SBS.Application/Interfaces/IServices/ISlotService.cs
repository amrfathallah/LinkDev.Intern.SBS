using SBS.Domain.Entities;
using SBS.Application.DTOs.Common;
using SBS.Application.DTOs.SlotDto;

namespace SBS.Application.Interfaces.IServices
{
	public interface ISlotService
	{
		Task<ApiResponse<List<SlotDto>>> GetAllSlots();
	}
}
