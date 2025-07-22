import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { FormBuilder, FormGroup } from '@angular/forms';

export interface Booking {
  id: number;
  resourceName: string;
  resourceType: string;
  userName: string;
  startTime: Date;
  endTime: Date;
  status: 'upcoming' | 'happening' | 'finished' | 'cancelled';
  attendees: number;
}

@Component({
  selector: 'app-bookings-management',
  templateUrl: './bookings-management.component.html',
  styleUrls: ['./bookings-management.component.css']
})
export class BookingsManagementComponent implements OnInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  filterForm: FormGroup;
  bookingsDisplayedColumns: string[] = ['resourceName', 'resourceType', 'userName', 'startTime', 'endTime', 'status'];
  bookingsDataSource = new MatTableDataSource<Booking>();

  constructor(
    public dialog: MatDialog,
    private fb: FormBuilder
  ) {
    this.filterForm = this.fb.group({
      startDate: [''],
      endDate: [''],
      resourceType: [''],
      user: [''],
      status: ['']
    });
  }

  ngOnInit() {
    this.loadBookingsData();
  }

  ngAfterViewInit() {
    this.bookingsDataSource.paginator = this.paginator;
    this.bookingsDataSource.sort = this.sort;
  }

  applyBookingsFilter() {
    const filterValues = this.filterForm.value;
    // Implement filtering logic here
    console.log('Applying filters:', filterValues);
  }

  clearFilters() {
    this.filterForm.reset();
    this.bookingsDataSource.filter = '';
  }

  refreshData() {
    this.loadBookingsData();
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'upcoming': return 'status-upcoming';
      case 'happening': return 'status-happening';
      case 'finished': return 'status-finished';
      case 'cancelled': return 'status-cancelled';
      default: return '';
    }
  }

  editBooking(booking: Booking) {
    console.log('Edit booking:', booking);
    // Implement edit booking dialog
  }

  cancelBooking(booking: Booking) {
    console.log('Cancel booking:', booking);
    // Implement cancel booking logic
  }

  viewBooking(booking: Booking) {
    console.log('View booking:', booking);
    // Implement view booking dialog
  }

  private loadBookingsData() {
    // Mock data - replace with actual API call
    const mockBookings: Booking[] = [
      {
        id: 1,
        resourceName: 'Conference Room A',
        resourceType: 'room',
        userName: 'John Doe',
        startTime: new Date('2025-07-15T09:00:00'),
        endTime: new Date('2025-07-15T10:00:00'),
        status: 'upcoming',
        attendees: 5
      },
      {
        id: 2,
        resourceName: 'Desk 12',
        resourceType: 'desk',
        userName: 'Sarah Mitchell',
        startTime: new Date('2025-07-15T08:00:00'),
        endTime: new Date('2025-07-15T17:00:00'),
        status: 'happening',
        attendees: 1
      },
      {
        id: 3,
        resourceName: 'Meeting Room B',
        resourceType: 'room',
        userName: 'Mike Johnson',
        startTime: new Date('2025-07-14T14:00:00'),
        endTime: new Date('2025-07-14T15:30:00'),
        status: 'finished',
        attendees: 3
      },
      {
        id: 4,
        resourceName: 'Conference Room C',
        resourceType: 'room',
        userName: 'Emily Davis',
        startTime: new Date('2025-07-15T11:00:00'),
        endTime: new Date('2025-07-15T12:00:00'),
        status: 'cancelled',
        attendees: 8
      }
    ];
    this.bookingsDataSource.data = mockBookings;
  }
}
