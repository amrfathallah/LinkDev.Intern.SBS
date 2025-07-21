export interface GetResourceDto {
  id: string;
  name: string;
  typeId: number;
  type: string;
  capacity: number;
  isActive: boolean;
  openAt: string; // format: "HH:mm:ss"
  closeAt: string; // format: "HH:mm:ss"
}
