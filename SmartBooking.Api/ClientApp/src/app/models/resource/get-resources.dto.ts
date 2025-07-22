export interface GetResourceDto {
  id: string;
  name: string;
  typeId: number;
  type: string;
  capacity: number;
  isActive: boolean;
  openAt: string;
  closeAt: string;
}
