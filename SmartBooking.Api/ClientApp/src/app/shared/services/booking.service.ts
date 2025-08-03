import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { Pagination } from '../models/pagination.model';
import {
  ViewAllBookingDto,
  ViewBookingsParams,
} from '../../admin-dashboard/components/bookings-management/models/booking-dtos.model';

@Injectable({
  providedIn: 'root',
})
export class BookingService {
  private baseUrl = `${environment.apiBaseUrl}/booking`;

  constructor(private http: HttpClient) {}

  getAllBookings(
    params?: ViewBookingsParams
  ): Observable<Pagination<ViewAllBookingDto>> {
    const requestBody = params || {
      pageIndex: 1,
      pageSize: 10,
    };

    return this.http
      .post<Pagination<ViewAllBookingDto>>(
        `${this.baseUrl}/allBooking`,
        requestBody,
        {
          headers: this.getAuthHeaders(),
        }
      )
      .pipe(catchError(this.handleError));
  }

  // Get all resource types for filtering
  getResourceTypes(): Observable<{ id: number; name: string }[]> {
    return this.http
      .get<{ id: number; name: string }[]>(
        `${environment.apiBaseUrl}/resource/ResourceTypes`,
        {
          headers: this.getAuthHeaders(),
        }
      )
      .pipe(catchError(this.handleError));
  }

  // Get all booking statuses for filtering
  getBookingStatuses(): Observable<{ id: number; name: string }[]> {
    return this.http
      .get<{ id: number; name: string }[]>(
        `${environment.apiBaseUrl}/booking/allBookingStatus`,
        {
          headers: this.getAuthHeaders(),
        }
      )
      .pipe(catchError(this.handleError));
  }

  // Get all users with bookings for filtering
  getUsersWithBookings(): Observable<{ id: string; name: string }[]> {
    return this.http
      .get<{ id: string; fullName: string }[]>(
        `${environment.apiBaseUrl}/booking/users-with-bookings`,
        {
          headers: this.getAuthHeaders(),
        }
      )
      .pipe(
        map((users: { id: string; fullName: string }[]) =>
          users.map((user: { id: string; fullName: string }) => ({
            id: user.id,
            name: user.fullName,
          }))
        ),
        catchError(this.handleError)
      );
  }

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({
      Authorization: token ? `Bearer ${token}` : '',
      'Content-Type': 'application/json',
    });
  }

  private handleError(error: any): Observable<never> {
    console.error('BookingService Error:', error);
    return throwError(() => error);
  }
}
