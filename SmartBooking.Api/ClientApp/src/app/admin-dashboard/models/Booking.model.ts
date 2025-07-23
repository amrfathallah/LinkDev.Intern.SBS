import { BookingStatus } from "../enums/BookingStatus.enum";

export interface Booking {
  id: number;
  resourceName: string;
  resourceType: string;
  userName: string;
  startTime: Date;
  endTime: Date;
  status: BookingStatus;
  attendees: number;
}
