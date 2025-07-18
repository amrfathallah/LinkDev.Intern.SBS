using Microsoft.AspNetCore.Mvc;
using SBS.Application.DTOs;
using SBS.Application.Interfaces.IServices;

namespace SmartBooking.Api.Controllers
{

	[ApiController]
	[Route("api/[Controller]")]
	public class BookingController : ControllerBase //To be changed to apiControllerBase
	{
		private readonly IBookingService _bookingService;

		public BookingController(IBookingService bookingService)
		{
			_bookingService = bookingService;
		}

		[HttpPost]
		[Route("book")]
		public async Task<IActionResult> BookResource([FromBody] BookingRequestDto bookingRequestDto)
		{
			//Check for the model binding
			if (!ModelState.IsValid)
			{
				//Get all errors of all properties
				string errorList = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors).Select(err => err.ErrorMessage));

				return StatusCode(400, errorList);
			}

			
			var result = await _bookingService.BookAsync(bookingRequestDto, new Guid(), "test"); //To be completed: Get userId from token

			if (!result)
			{
				return StatusCode(409);
			}
			
			return StatusCode(201);
		}

	}
}
