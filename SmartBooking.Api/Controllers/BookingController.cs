using Microsoft.AspNetCore.Mvc;
using SBS.Application.DTOs.BookingDto;
using SBS.Application.DTOs.Common;
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

					return BadRequest(new ApiResponse
					{
						Success = false,
						Message = errorList
					});
				}


				//Extract info from token
				

				var result = await _bookingService.BookAsync(bookingRequestDto,Guid.Parse("3BECB38E-CC36-431D-EE02-08DDCE8742FC"), "testUser"); //To be completed: Get userId and username from token
			
				if (result)
				{
					return Ok(new ApiResponse
					{
						Success = true,
						Message = "Booking completed successfully."
					});
				}
				else
				{
					return BadRequest(new ApiResponse
					{
						Success = false,
						Message = "Booking failed due to slot conflict or unavailability."
					});
				}
			}
			catch(Exception)
			{
				return BadRequest(new ApiResponse
				{
					Success = false,
					Message = "An error occurred while processing your request."
				});
			}
			
		}

	}
}
