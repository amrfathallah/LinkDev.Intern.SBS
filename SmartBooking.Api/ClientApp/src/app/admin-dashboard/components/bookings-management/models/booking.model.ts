import { BookingStatus } from '../../../enums/BookingStatus.enum';

export interface Booking {
  id: string;
  resourceName: string;
  resourceType: string;
  userName: string;
  startTime: Date;
  endTime: Date;
  status: string;
  date: Date;
}
