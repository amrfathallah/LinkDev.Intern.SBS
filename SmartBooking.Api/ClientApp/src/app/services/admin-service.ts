import { GetResourceDto } from './../models/resource/get-resources.dto';
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { CreateResourceDto } from "../models/resource/create-resource.dto";
import { UpdateResourceDto } from "../models/resource/update-resource.dto";
@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private _httpClient:HttpClient) {}

  createResource(resource: CreateResourceDto) {
    return this._httpClient.post(`${environment.apiUrl}/api/Resource`, resource);
  }
  getResources() {
    return this._httpClient.get<GetResourceDto[]>(`${environment.apiUrl}/api/Resource`);
  }
  updateResource(id: string, resource: UpdateResourceDto) {
    return this._httpClient.put(`${environment.apiUrl}/api/Resource/${id}`, resource);
  }
  deleteResource(id: string){
    return this._httpClient.delete(`${environment.apiUrl}/api/Resource/${id}`);
  }
}
