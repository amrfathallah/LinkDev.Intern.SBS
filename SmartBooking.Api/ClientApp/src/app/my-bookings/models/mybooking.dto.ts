import { BookingStatus } from '../enums/BookingStatus.enum';

export interface MyBookingDto {
  id: string;
  resourceId: string;
  date: Date;
  status: BookingStatus;
  startTime: string;
  endTime: string;
}
