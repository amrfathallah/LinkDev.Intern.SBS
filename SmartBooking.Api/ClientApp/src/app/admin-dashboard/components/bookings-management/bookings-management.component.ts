import { Component, OnInit } from '@angular/core';
import { Sort } from '@angular/material/sort';
import { Booking } from './models/booking.model';
import { BookingsManagementService } from './services/bookings-management.service';
import { BookingService } from '../../../shared/services/booking.service';
import { ViewBookingsParams } from './models/booking-dtos.model';

@Component({
  selector: 'app-bookings-management',
  templateUrl: './bookings-management.component.html',
  styleUrls: ['./bookings-management.component.css'],
})
export class BookingsManagementComponent implements OnInit {
  // Data properties
  bookings: Booking[] = [];
  totalCount = 0;
  currentPageIndex = 0;
  currentPageSize = 10;
  isLoading = false;
  errorMessage = '';

  // Sorting properties
  currentSortBy = 'date';
  currentSortColumn = 'date'; // Original frontend column name
  currentSortDirection: 'asc' | 'desc' | '' = 'asc';

  // Filter data sources
  uniqueUsers: { id: string; name: string }[] = [];
  allResourceTypes: { id: number; name: string }[] = [];
  allBookingStatuses: { id: number; name: string }[] = [];

  constructor(
    private bookingsService: BookingsManagementService,
    private bookingService: BookingService
  ) {}

  ngOnInit() {
    this.loadStaticFilterData();
    this.loadBookingsData();
  }

  private loadStaticFilterData() {
    // Load resource types
    this.bookingService.getResourceTypes().subscribe({
      next: (resourceTypes: { id: number; name: string }[]) => {
        this.allResourceTypes = resourceTypes;
      },
      error: (error: any) => {
        console.error('Error loading resource types:', error);
        this.allResourceTypes = [];
      },
    });

    // Load booking statuses
    this.bookingService.getBookingStatuses().subscribe({
      next: (statuses: { id: number; name: string }[]) => {
        this.allBookingStatuses = statuses;
      },
      error: (error: any) => {
        console.error('Error loading booking statuses:', error);
        this.allBookingStatuses = [];
      },
    });

    // Load users with bookings
    this.bookingService.getUsersWithBookings().subscribe({
      next: (users: { id: string; name: string }[]) => {
        this.uniqueUsers = users;
      },
      error: (error: any) => {
        console.error('Error loading users with bookings:', error);
        this.uniqueUsers = [];
      },
    });
  }

  onFiltersApplied(filterParams: any) {
    this.currentPageIndex = 0;
    this.loadBookingsData(filterParams);
  }

  onFiltersCleared() {
    this.currentPageIndex = 0;
    this.loadBookingsData();
  }

  onSortChanged(sortState: Sort) {
    this.currentPageIndex = 0;
    this.currentSortColumn = sortState.active; // Store original column name
    this.currentSortBy = this.bookingsService.mapSortField(sortState.active); // Store mapped field
    this.currentSortDirection = sortState.direction;
    this.loadBookingsData();
  }

  onPageChanged(pageEvent: { pageIndex: number; pageSize: number }) {
    this.currentPageIndex = pageEvent.pageIndex;
    this.currentPageSize = pageEvent.pageSize;
    this.loadBookingsData();
  }

  refreshData() {
    this.loadBookingsData();
  }

  private loadBookingsData(filterParams?: any) {
    this.isLoading = true;
    this.errorMessage = '';

    const params: ViewBookingsParams = {
      pageIndex: this.currentPageIndex,
      pageSize: this.currentPageSize,
      sortBy: this.currentSortBy,
      isDescending: this.currentSortDirection === 'desc',
      ...filterParams,
    };

    this.bookingsService.getBookings(params).subscribe({
      next: (result) => {
        this.bookings = result.data;
        this.totalCount = result.count;
        this.currentPageIndex = result.pageIndex;
        this.currentPageSize = result.pageSize;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading bookings:', error);
        this.isLoading = false;
        this.errorMessage = 'Failed to load bookings. Please try again.';
        this.bookings = [];
        this.totalCount = 0;
      },
    });
  }
}
