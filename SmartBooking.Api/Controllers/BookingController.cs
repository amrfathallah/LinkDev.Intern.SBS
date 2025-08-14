using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.TeamFoundation.TestManagement.WebApi;
using SBS.Application.DTOs;
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

		[Authorize]
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
			catch (Exception)
			{
				return BadRequest(new ApiResponse
				{
					Success = false,
					Message = "An error occurred while processing your request."
				});
			}
		}
		[Authorize]
		[HttpGet]
		[Route("get-my-bookings")]
		public async Task<IActionResult> GetBookingsByUser()
		{
			try
			{
				var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
				var bookings = await _bookingService.GetBookingsByUserAsync(Guid.Parse(userID));
				return Ok(new ApiResponse<List<MyBookingDto>>
				{
					Success = true,
					Data = bookings
				});
			}
			catch (Exception ex)
			{
				return BadRequest(new ApiResponse
				{
					Success = false,
					Message = $"An error occurred while retrieving bookings: {ex.Message}"
				});
			}
		}

		[Authorize]
		[HttpDelete]
		[Route("cancel-booking/{bookingId}")]
		public async Task<IActionResult> CancelBooking(Guid bookingId)
		{
			try
			{
				var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
				var result = await _bookingService.CancelBookingAsync(bookingId, Guid.Parse(userID));
				if (result)
				{
					return Ok(new ApiResponse
					{
						Success = true,
						Message = "Booking cancelled successfully."
					});
				}
				else
				{
					return BadRequest(new ApiResponse
					{
						Success = false,
						Message = "Failed to cancel booking."
					});
				}
			}
			catch (Exception ex)
			{
				return BadRequest(new ApiResponse
				{
					Success = false,
					Message = $"An error occurred while cancelling the booking: {ex.Message}"
				});
			}
		}

		//[Authorize(Roles = "Admin")]
		[HttpPost("allBooking")]
		public async Task<IActionResult> GetAllBookings([FromBody] ViewBookingsParams ViewBookingquery)
		{
			try
			{

				var result = await _bookingService.GetAllBookingsAsync(ViewBookingquery);
				return Ok(result);
			}
			catch (Exception)
			{

				return StatusCode(500, "An error occurred while fetching bookings.");
			}
		}

		[HttpGet("allBookingStatus")]
		public async Task<IActionResult> GetAllBookingStatus()
		{
			var result = await _bookingService.GetAllBookingStatusAsync();
			return Ok(result);
		}

		[HttpGet("users-with-bookings")]
		public async Task<IActionResult> GetUsersWithBookings()
		{
			var users = await _bookingService.GetUsersWithBookingsAsync();
			return Ok(users);
		}
	}
}
