

using Microsoft.AspNetCore.Mvc;
using SBS.Application.DTOs.Common;
using SBS.Application.DTOs.SlotDto;
using SBS.Application.Interfaces.IServices;
using SBS.Domain.Entities;

namespace SmartBooking.Api.Controllers
{
	[ApiController]
	[Route("api/[Controller]")]
	public class SlotController : ControllerBase //To be Completed: change to apiControllerBase
	{
		private readonly ISlotService _slotService;

		public SlotController(ISlotService slotService)
		{
			_slotService = slotService;
		}

		[HttpGet]
		[Route("getAll")]
		public async Task<ActionResult<ApiResponse<List<SlotDto>>>> GetAllSlots()
		{
			var response = await _slotService.GetAllSlots();
			return Ok(response);
		}
	}
}
