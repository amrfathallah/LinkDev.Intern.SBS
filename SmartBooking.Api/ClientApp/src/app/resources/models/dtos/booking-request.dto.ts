export interface BookingRequestDto {
  resourceId: string;
  date: string; // YYYY-MM-DD format
  slotsIds: number[]; // Array of slot IDs to book
}
