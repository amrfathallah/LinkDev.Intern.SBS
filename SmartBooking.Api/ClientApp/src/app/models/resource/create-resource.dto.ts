export interface CreateResourceDto {
  name: string;
  typeId: number;
  capacity: number;
  openAt: string; // "HH:mm:ss" format
  closeAt: string; // "HH:mm:ss" format
}
