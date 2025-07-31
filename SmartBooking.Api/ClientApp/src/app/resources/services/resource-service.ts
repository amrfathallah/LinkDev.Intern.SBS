import { Injectable } from "@angular/core";

import { HttpClient, HttpHeaders } from "@angular/common/http";
import { environment } from "src/environments/environment";
import { GetResourceDto } from "../models/dtos/get-resources.dto";
import { ResourceBookedSlotsDto } from "../models/dtos/resource-booked-slots.dto";
import { GetBookedSlotsRequestDto } from "../models/dtos/get-booked-slots-request.dto";
import { BookingRequestDto } from "../models/dtos/booking-request.dto";
import { ApiResponse } from "src/app/shared/models/api-response.model";
import { SlotDto } from "../models/dtos/slot.dto";
import { map, Observable } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class ResourceService {
  constructor(private _httpClient: HttpClient) {}

  getBookedSlots(resourceId: string, date: string) {
    const request: GetBookedSlotsRequestDto = {
      resourceId: resourceId,
      date: date
    };

    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    console.log('Request payload:', JSON.stringify(request, null, 2)); // Debug log

    return this._httpClient.post<ResourceBookedSlotsDto>(
      `${environment.apiBaseUrl}/Resource/booked-slots`,
      request,
      { headers }
    ).pipe(
      map(response => {
        console.log('Booked slots fetched:', response);
        return response;
      })
    );
  }

  getAvailableResources(date: Date): Observable<GetResourceDto[]> {
    // Format date as YYYY-MM-DD
    const formattedDate = date.toISOString().split('T')[0];
    
    return this._httpClient.get<GetResourceDto[]>(
      `${environment.apiBaseUrl}/Resource/available?date=${formattedDate}`
    );
  }

  getResources() {
    // Use today's date as default
    const today = new Date();
    return this.getAvailableResources(today);
  }

  getResourceById(id: string) {
    return this._httpClient.get<GetResourceDto>(
      `${environment.apiBaseUrl}/Resource/${id}`
    );
  }

  bookSlots(bookingData: BookingRequestDto) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    console.log('Booking request payload:', JSON.stringify(bookingData, null, 2)); // Debug log

    return this._httpClient.post<ApiResponse<any>>(
      `${environment.apiBaseUrl}/Booking/book`,
      bookingData,
      { headers }
    );
  }
  getSlots():Observable<ApiResponse<SlotDto[]>> {
    return this._httpClient.get<ApiResponse<SlotDto[]>>(
      `${environment.apiBaseUrl}/Slot/getAll`
    );
  }
}
