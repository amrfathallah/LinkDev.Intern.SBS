import { ResourceType } from "../enums/ResourceType.enum";

export interface Resource {
  id: string;
  name: string;
  type: string;
  typeId: ResourceType;
  capacity: number;
  active: boolean;
  openAt: string;
  closeAt: string;
}
