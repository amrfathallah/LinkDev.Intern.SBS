import {
  Component,
  EventEmitter,
  Input,
  Output,
  ViewChild,
  OnInit,
  AfterViewInit,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { BookingStatus } from '../../../enums/BookingStatus.enum';
import { Booking } from '../models/booking.model';
import * as BookingHelpers from '../helpers/booking-helpers';

@Component({
  selector: 'app-booking-table',
  templateUrl: './booking-table.component.html',
  styleUrls: ['./booking-table.component.css'],
})
export class BookingTableComponent implements OnInit, AfterViewInit, OnChanges {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  @Input() bookings: Booking[] = [];
  @Input() totalCount = 0;
  @Input() currentPageIndex = 0;
  @Input() currentPageSize = 10;
  @Input() isLoading = false;
  @Input() currentSortBy = 'date';
  @Input() currentSortDirection: 'asc' | 'desc' | '' = 'asc';

  @Output() sortChanged = new EventEmitter<Sort>();
  @Output() pageChanged = new EventEmitter<{
    pageIndex: number;
    pageSize: number;
  }>();

  // Dynamic pagination configuration
  paginationConfig = {
    pageSizeOptions: [10, 25, 50, 100],
    defaultPageSize: 10,
  };

  // Dynamic column headers from backend configuration
  columnHeaders = {
    resourceName: 'Resource',
    resourceType: 'Type',
    userName: 'User',
    date: 'Date',
    startTime: 'Start Time',
    endTime: 'End Time',
    status: 'Status',
  };

  bookingsDisplayedColumns: string[] = [
    'resourceName',
    'resourceType',
    'userName',
    'date',
    'startTime',
    'endTime',
    'status',
  ];

  bookingsDataSource = new MatTableDataSource<Booking>();

  constructor() {}

  ngOnInit() {
    this.bookingsDataSource.data = this.bookings;
  }

  ngAfterViewInit() {
    setTimeout(() => {
      this.setupTableControls();
    });
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['bookings']) {
      this.bookingsDataSource.data = this.bookings;
    }

    // Update paginator info when data changes
    if (
      this.paginator &&
      (changes['totalCount'] ||
        changes['currentPageIndex'] ||
        changes['currentPageSize'])
    ) {
      this.paginator.length = this.totalCount;
      this.paginator.pageIndex = this.currentPageIndex;
      this.paginator.pageSize = this.currentPageSize;
    }

    // Update sort state when sort properties change
    if (
      this.sort &&
      (changes['currentSortBy'] || changes['currentSortDirection'])
    ) {
      this.sort.active = this.currentSortBy;
      this.sort.direction = this.currentSortDirection;
    }
  }

  private setupTableControls() {
    // Setup sort
    if (this.sort) {
      // Disable client-side sorting since we're doing server-side
      this.bookingsDataSource.sortingDataAccessor = () => '';

      // Set initial sort state based on inputs
      this.sort.active = this.currentSortBy;
      this.sort.direction = this.currentSortDirection;

      // Don't connect to dataSource for server-side sorting
      this.bookingsDataSource.sort = null;
    }

    // Setup paginator for server-side pagination
    if (this.paginator) {
      // Don't connect to dataSource for server-side pagination
      this.bookingsDataSource.paginator = null;

      // Set initial paginator values
      this.paginator.length = this.totalCount;
      this.paginator.pageIndex = this.currentPageIndex;
      this.paginator.pageSize = this.currentPageSize;
    }
  }

  onSortChange(sortState: Sort) {
    // Custom 2-way toggle logic
    if (sortState.active === this.currentSortBy) {
      // Same column clicked - toggle direction
      if (this.currentSortDirection === 'asc') {
        sortState.direction = 'desc';
      } else {
        sortState.direction = 'asc';
      }
    } else {
      // Different column clicked - start with asc
      sortState.direction = 'asc';
    }

    // Update the sort UI
    if (this.sort) {
      this.sort.direction = sortState.direction;
      this.sort.active = sortState.active;
    }

    this.sortChanged.emit(sortState);
  }

  getStatusClass(status: string): string {
    return BookingHelpers.getStatusClass(status);
  }

  onPageEvent(event: any) {
    this.pageChanged.emit({
      pageIndex: event.pageIndex,
      pageSize: event.pageSize,
    });
  }
}
