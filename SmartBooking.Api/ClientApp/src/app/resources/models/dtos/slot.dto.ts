export interface SlotDto {
  id: string;
  startTime: string;
  endTime: string;
}

// for component's internal slot representation
export interface ComponentSlotDto extends SlotDto {
  isActive: boolean;
  isBooked: boolean;
}
