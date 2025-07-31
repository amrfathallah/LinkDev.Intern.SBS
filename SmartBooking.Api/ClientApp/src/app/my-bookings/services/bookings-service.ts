import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { ApiResponse } from 'src/app/shared/models/api-response.model';
import { map, Observable } from 'rxjs';
import { MyBookingDto } from '../models/mybooking.dto';

@Injectable({
  providedIn: 'root',
})
export class BookingsService {
  constructor(private _httpClient: HttpClient) {}

  getUserBookings() {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return this._httpClient.get<ApiResponse<MyBookingDto[]>>(
      `${environment.apiBaseUrl}/Booking/get-my-bookings`,
      { headers }
    );
  }

  deleteBooking(bookingId: string) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return this._httpClient.delete<ApiResponse>(
      `${environment.apiBaseUrl}/Booking/cancel-booking/${bookingId}`,
      { headers }
    );
  }
}
