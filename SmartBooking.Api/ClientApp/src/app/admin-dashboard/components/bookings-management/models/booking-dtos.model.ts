export interface ViewAllBookingDto {
  id: string;
  user: string;
  date: string;
  resourceName: string;
  resourceType: string;
  status: string;
  startTime: string;
  endTime: string;
}

export interface ViewBookingsParams {
  pageIndex?: number;
  pageSize?: number;
  userId?: string; // Will be converted to Guid on backend
  resourceTypeId?: number;
  bookingStatusId?: number;
  from?: string;
  to?: string;
  sortBy?: string;
  isDescending?: boolean;
}
