import { Booking } from '../models/booking.model';
import { ViewAllBookingDto } from '../models/booking-dtos.model';
import { BookingStatus } from '../../../enums/BookingStatus.enum';

// Core mapping functions that can be reused across the app
export function parseTimeSpan(date: string, timeSpan: string): Date {
  const dateObj = new Date(date);
  const [hours, minutes, seconds] = timeSpan
    .split(':')
    .map((num) => parseInt(num));
  dateObj.setHours(hours, minutes, seconds || 0, 0);
  return dateObj;
}

export function formatDateForApi(date: Date): string {
  if (!date) return '';
  return date.toISOString().split('T')[0];
}

// Main DTO to Model mapper - used across different components
export function mapDtoToBooking(dto: ViewAllBookingDto): Booking {
  return {
    id: dto.id,
    resourceName: dto.resourceName,
    resourceType: dto.resourceType,
    userName: dto.user,
    startTime: parseTimeSpan(dto.date, dto.startTime),
    endTime: parseTimeSpan(dto.date, dto.endTime),
    status: dto.status,
    date: new Date(dto.date),
  };
}

// Status styling utility - maps status string to CSS class
export function getStatusClass(status: string): string {
  switch (status.toLowerCase()) {
    case BookingStatus.Upcoming:
      return 'status-upcoming';
    case BookingStatus.Happening:
      return 'status-happening';
    case BookingStatus.Finished:
      return 'status-finished';
    default:
      return 'status-unknown';
  }
}
