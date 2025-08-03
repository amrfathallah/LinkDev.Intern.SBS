import { BookingStatus } from '../../../enums/BookingStatus.enum';
import { Booking } from '../models/booking.model';
import { ViewAllBookingDto } from '../models/booking-dtos.model';

// Core mapping functions that can be reused across the app
export function parseTimeSpan(date: string, timeSpan: string): Date {
  const dateObj = new Date(date);
  const [hours, minutes, seconds] = timeSpan
    .split(':')
    .map((num) => parseInt(num));
  dateObj.setHours(hours, minutes, seconds || 0, 0);
  return dateObj;
}

export function parseStatus(status: string): BookingStatus {
  switch (status.toLowerCase()) {
    case 'upcoming':
      return BookingStatus.Upcoming;
    case 'happening':
      return BookingStatus.Happening;
    case 'finished':
      return BookingStatus.Finished;
    default:
      return BookingStatus.Upcoming;
  }
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
    status: parseStatus(dto.status),
    date: new Date(dto.date),
  };
}

// Status display utilities
export function getStatusDisplayName(status: BookingStatus): string {
  switch (status) {
    case BookingStatus.Upcoming:
      return 'Upcoming';
    case BookingStatus.Happening:
      return 'In Progress';
    case BookingStatus.Finished:
      return 'Completed';
    default:
      return 'Unknown';
  }
}

export function getStatusClass(status: BookingStatus): string {
  switch (status) {
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
