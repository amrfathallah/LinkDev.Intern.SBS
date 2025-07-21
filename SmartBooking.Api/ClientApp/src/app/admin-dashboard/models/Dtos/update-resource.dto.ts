export interface UpdateResourceDto {
  name: string;
  capacity: number;
  openAt: string; // "HH:mm:ss" format
  closeAt: string; // "HH:mm:ss" format
  isActive: boolean;
}
