export interface BookedSlotDto {
  slotId: number;
  startTime: string;
  endTime: string;
}

export interface ResourceBookedSlotsDto {
  resourceId: string;
  resourceName: string;
  bookedSlots: BookedSlotDto[];
}
