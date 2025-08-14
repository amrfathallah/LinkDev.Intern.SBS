import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { BookingService } from '../../../../shared/services/booking.service';
import { Booking } from '../models/booking.model';
import {
  ViewAllBookingDto,
  ViewBookingsParams,
} from '../models/booking-dtos.model';
import { Pagination } from '../../../../shared/models/pagination.model';
import * as BookingHelpers from '../helpers/booking-helpers';

@Injectable({
  providedIn: 'root',
})
export class BookingsManagementService {
  constructor(private bookingService: BookingService) {}

  // Main method to get bookings with proper mapping
  getBookings(params: ViewBookingsParams): Observable<Pagination<Booking>> {
    const apiParams: ViewBookingsParams = {
      pageIndex: (params.pageIndex ?? 0) + 1, // API expects 1-based indexing
      pageSize: params.pageSize ?? 10,
      sortBy: this.mapSortField(params.sortBy || ''), // Map frontend fields to backend
      isDescending: params.isDescending,
    };

    // Add optional filters
    if (params.from) apiParams.from = params.from;
    if (params.to) apiParams.to = params.to;
    if (params.resourceTypeId) apiParams.resourceTypeId = params.resourceTypeId;
    if (params.bookingStatusId)
      apiParams.bookingStatusId = params.bookingStatusId;
    if (params.userId) apiParams.userId = params.userId;

    return this.bookingService.getAllBookings(apiParams).pipe(
      map((response) => {
        if (response && response.data) {
          const bookings: Booking[] = response.data.map(
            (dto: ViewAllBookingDto) => BookingHelpers.mapDtoToBooking(dto)
          );

          return {
            data: bookings,
            count: response.count,
            pageIndex: response.pageIndex - 1, // Convert back to 0-based
            pageSize: response.pageSize,
          } as Pagination<Booking>;
        } else {
          return {
            data: [],
            count: 0,
            pageIndex: 0,
            pageSize: params.pageSize ?? 10,
          } as Pagination<Booking>;
        }
      })
    );
  }

  // Map frontend column names to backend field names
  mapSortField(frontendField: string): string {
    const sortMapping: { [key: string]: string } = {
      resourceName: 'resource', // Backend expects 'resource' for Resource.Name
      resourceType: 'resource', // Backend expects 'resource' for Resource.Type
      userName: 'user', // Backend expects 'user' for User.FullName
      date: 'date', // Backend expects 'date' for Booking.Date
      startTime: 'date', // Backend sorts by date, not separate time fields
      endTime: 'date', // Backend sorts by date, not separate time fields
      status: '', // Backend doesn't support status sorting, falls back to default
    };

    return sortMapping[frontendField] || frontendField;
  }
}
