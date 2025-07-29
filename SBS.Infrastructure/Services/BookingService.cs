using SBS.Application.Interfaces;
using SBS.Application.Interfaces.IServices;
using SBS.Domain.Entities;
using SBS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SBS.Application.DTOs.BookingDto;
using SBS.Application.DTOs.Common;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace SBS.Application.Services
{
	public class BookingService : IBookingService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IBookingConflictValidator _conflictValidator;
		private readonly IMapper _mapper;

		public BookingService(IUnitOfWork unitOfWork, IBookingConflictValidator bookingConflictValidator, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_conflictValidator = bookingConflictValidator;
			_mapper = mapper;
		}

		public async Task<bool> BookAsync(BookingRequestDto requestDto, Guid userId, string createdBy)
		{
			await _unitOfWork.BeginTransactionAsync();

			//Input Validation
			if (await _unitOfWork.Resources.GetByIdAsync(requestDto.ResourceId) == null)
			{
				throw new Exception("Resource doesn't exist");
			}

			var slots = await _unitOfWork.SlotRepository.GetByIdsAsync(requestDto.SlotsIds);
			if (slots.Count != requestDto.SlotsIds.Count)
			{
				throw new Exception("Invalid slots are selected");
			}

			if (requestDto.Date < DateOnly.FromDateTime(DateTime.Today))
			{
				throw new Exception("Can't book a resource in the past");
			}


			//Check for booking conflicts
			if (await _conflictValidator.HasConflictAsync(requestDto.ResourceId, requestDto.Date, requestDto.SlotsIds))
			{
				return false;
			}


			//Booking Logic
			var booking = new Booking
			{
				UserId = userId,
				ResourceId = requestDto.ResourceId,
				Date = requestDto.Date,
				StatusId = (int)BookingStatusEnum.Upcoming,
				CreatedAt = DateTime.UtcNow,
				CreatedBy = createdBy,

			};

			await _unitOfWork.Bookings.AddAsync(booking);
			await _unitOfWork.CommitAsync();

			var bookingSlots = requestDto.SlotsIds.Select(slotId => new BookingSlot
			{
				SlotId = slotId,
				BookingId = booking.Id,
			}).ToList();


			await _unitOfWork.BookingSlotRepository.AddRangeAsync(bookingSlots);
			await _unitOfWork.CommitAsync();

			await _unitOfWork.CommitTransactionAsync();

			return true;




		}

		public async Task<Pagination<ViewAllBookingDto>> GetAllBookingsAsync(ViewBookingsParams viewBookingsParams)
		{


			try
			{
				var allBookings = _unitOfWork.Bookings.GetAllBookingWithIncludes();

				allBookings = ApplyFilters(allBookings, viewBookingsParams);
				var totalCount = await allBookings.CountAsync();
				allBookings = ApplySorting(allBookings, viewBookingsParams);

				var pagedData = await ApplyPagination(allBookings, viewBookingsParams)
					.ToListAsync();

				var bookingsDto = _mapper.Map<List<ViewAllBookingDto>>(pagedData);

				return new Pagination<ViewAllBookingDto>(viewBookingsParams.PageIndex, viewBookingsParams.PageSize, totalCount) { Data = bookingsDto };

			}
			catch (Exception ex)
			{

				throw new ApplicationException("An error occurred while retrieving bookings.", ex);
			}

		}

		// Helper methods for BookingService

		private static IQueryable<Booking> ApplyFilters(IQueryable<Booking> query, ViewBookingsParams viewBookingsFilter)
		{
			if (viewBookingsFilter.UserId.HasValue)
				query = query.Where(b => b.UserId == viewBookingsFilter.UserId);

			if (viewBookingsFilter.ResourceTypeId.HasValue)
				query = query.Where(b => b.Resource!.TypeId == viewBookingsFilter.ResourceTypeId);

			if (viewBookingsFilter.BookingStatusId.HasValue)
				query = query.Where(b => b.StatusId == viewBookingsFilter.BookingStatusId);

			if (viewBookingsFilter.From.HasValue)
				query = query.Where(b => b.Date >= viewBookingsFilter.From.Value);

			if (viewBookingsFilter.To.HasValue)
				query = query.Where(b => b.Date <= viewBookingsFilter.To.Value);

			return query;
		}

		private static IQueryable<Booking> ApplySorting(IQueryable<Booking> query, ViewBookingsParams viewBookingsparams)
		{
			return viewBookingsparams.SortBy?.ToLower() switch
			{
				"date" => viewBookingsparams.IsDescending ? query.OrderByDescending(b => b.Date) : query.OrderBy(b => b.Date),
				"user" => viewBookingsparams.IsDescending ? query.OrderByDescending(b => b.User!.FullName) : query.OrderBy(b => b.User!.FullName),
				"resource" => viewBookingsparams.IsDescending ? query.OrderByDescending(b => b.Resource!.Name) : query.OrderBy(b => b.Resource!.Name),
				_ => query.OrderByDescending(b => b.CreatedAt)
			};
		}

		private static IQueryable<Booking> ApplyPagination(IQueryable<Booking> query, ViewBookingsParams viewBookingsParams)
		{
			var pageIndex = viewBookingsParams.PageIndex < 1 ? 1 : viewBookingsParams.PageIndex;

			return query.Skip(viewBookingsParams.PageSize * (pageIndex - 1))
						.Take(viewBookingsParams.PageSize);
		}


	}




}
