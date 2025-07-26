using Microsoft.AspNetCore.Mvc;
using SBS.Application.DTOs;
using SBS.Application.DTOs.BookingDto;
using SBS.Application.Interfaces.IServices;

namespace SmartBooking.Api.Controllers
{

	[ApiController]
	[Route("api/[Controller]")]
	public class BookingController : ControllerBase //To be Completed: change to apiControllerBase
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
			try
			{
				//Check for the model binding
				if (!ModelState.IsValid)
				{
					//Get all errors of all properties
					string errorList = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors).Select(err => err.ErrorMessage));

					return StatusCode(400, errorList);
				}


				//Extract info from token
				

				var result = await _bookingService.BookAsync(bookingRequestDto,Guid.Parse("6F8BBE7D-5A47-4D39-87AF-512E34F5E630"), "testUser"); //To be completed: Get userId and username from token

				if (!result)
				{
					return StatusCode(409);
				}

				return StatusCode(201);
			}
			catch(Exception)
			{
				return StatusCode(500, "Unexpected error occurred");
			}
			
		}

	}
}
