
using SBS.Application.DTOs.Common;
using SBS.Application.DTOs.SlotDto;
using SBS.Application.Interfaces.IRepositories;
using SBS.Application.Interfaces.IServices;
using SBS.Domain.Entities;

namespace SBS.Application.Services
{
	public class SlotService : ISlotService
	{
		private readonly ISlotRepository _slotRepository;

		public SlotService(ISlotRepository slotRepository) {
			_slotRepository = slotRepository;
		}

		public async Task<ApiResponse<List<SlotDto>>> GetAllSlots()
		{
			var slots =  await _slotRepository.GetAllAsync();
			var slotsDto = slots.Select(slot => new SlotDto
			{
				Id = slot.Id,
				StartTime = slot.StartTime,
				EndTime = slot.EndTime
			}).ToList();
			return new ApiResponse<List<SlotDto>> { Success = true, Message = "Slots returned", Data = slotsDto };
		}
	}
}
