import { GetResourceDto } from '../models/Dtos/get-resources.dto';
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { CreateResourceDto } from "../models/Dtos/create-resource.dto";
import { UpdateResourceDto } from "../models/Dtos/update-resource.dto";
@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private _httpClient:HttpClient) {}

  createResource(resource: CreateResourceDto) {
    return this._httpClient.post(`${environment.apiBaseUrl}/Resource`, resource);
  }
  getResources() {
    return this._httpClient.get<GetResourceDto[]>(`${environment.apiBaseUrl}/Resource`);
  }
  updateResource(id: string, resource: UpdateResourceDto) {
    return this._httpClient.put(`${environment.apiBaseUrl}/Resource/${id}`, resource);
  }
  deleteResource(id: string){
    return this._httpClient.delete(`${environment.apiBaseUrl}/Resource/${id}`);
  }
}
