import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { FormBuilder, FormGroup } from '@angular/forms';
import { BookingStatus } from '../../enums/BookingStatus.enum';
import { Booking } from '../../models/Booking.model';


@Component({
  selector: 'app-bookings-management',
  templateUrl: './bookings-management.component.html',
  styleUrls: ['./bookings-management.component.css']
})
export class BookingsManagementComponent implements OnInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  filterForm: FormGroup;


  // TODO: is this the best way to do it?
  // angular material requires a list of displayed columns
  bookingsDisplayedColumns: string[] = ['resourceName', 'resourceType', 'userName', 'startTime', 'endTime', 'status'];

  bookingsDataSource = new MatTableDataSource<Booking>();

  constructor(
    public dialog: MatDialog,
    private fb: FormBuilder
  ) {

    // TODO: figure out what is the best practices for making forms
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
    console.log('Applying filters:', filterValues);
  }

  clearFilters() {
    this.filterForm.reset();
    this.bookingsDataSource.filter = '';
  }

  refreshData() {
    this.loadBookingsData();
  }

  // this function returns a css class so it should be a string
  getStatusClass(status: BookingStatus): string {
    switch (status) {
      case BookingStatus.Upcoming: return 'status-upcoming';
      case BookingStatus.Happening: return 'status-happening';
      case BookingStatus.Finished: return 'status-finished';
      case BookingStatus.Cancelled: return 'status-cancelled';
      default: return '';
    }
  }

  private loadBookingsData() {
    const mockBookings: Booking[] = [
      {
        id: 1,
        resourceName: 'Conference Room A',
        resourceType: 'room',
        userName: 'John Doe',
        startTime: new Date('2025-07-15T09:00:00'),
        endTime: new Date('2025-07-15T10:00:00'),
        status: BookingStatus.Upcoming,
        attendees: 5
      },
      {
        id: 2,
        resourceName: 'Desk 12',
        resourceType: 'desk',
        userName: 'Sarah Mitchell',
        startTime: new Date('2025-07-15T08:00:00'),
        endTime: new Date('2025-07-15T17:00:00'),
        status: BookingStatus.Happening,
        attendees: 1
      },
      {
        id: 3,
        resourceName: 'Meeting Room B',
        resourceType: 'room',
        userName: 'Mike Johnson',
        startTime: new Date('2025-07-14T14:00:00'),
        endTime: new Date('2025-07-14T15:30:00'),
        status: BookingStatus.Finished,
        attendees: 3
      },
      {
        id: 4,
        resourceName: 'Conference Room C',
        resourceType: 'room',
        userName: 'Emily Davis',
        startTime: new Date('2025-07-15T11:00:00'),
        endTime: new Date('2025-07-15T12:00:00'),
        status: BookingStatus.Cancelled,
        attendees: 8
      }
    ];
    this.bookingsDataSource.data = mockBookings;
  }
}
