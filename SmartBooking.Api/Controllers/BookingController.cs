using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SBS.Application.DTOs.BookingDto;
using SBS.Application.DTOs.Common;
using SBS.Application.Interfaces.IServices;
using System.Security.Claims;

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
		[Authorize]
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
				var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
				var userName = User.FindFirst(ClaimTypes.Name)?.Value;
				var result = await _bookingService.BookAsync(bookingRequestDto, Guid.Parse(userID), userName);

				var result = await _bookingService.BookAsync(bookingRequestDto,Guid.Parse("53e90a26-db53-4cbb-f7bb-08ddc9d0ee59"), "testUser"); //To be completed: Get userId and username from token
			
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
