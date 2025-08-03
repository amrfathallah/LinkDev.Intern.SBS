import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import * as BookingHelpers from '../helpers/booking-helpers';

@Component({
  selector: 'app-booking-filters',
  templateUrl: './booking-filters.component.html',
  styleUrls: ['./booking-filters.component.css'],
})
export class BookingFiltersComponent implements OnInit {
  @Input() allResourceTypes: { id: number; name: string }[] = [];
  @Input() allBookingStatuses: { id: number; name: string }[] = [];
  @Input() uniqueUsers: { id: string; name: string }[] = [];
  @Output() filtersApplied = new EventEmitter<any>();
  @Output() filtersCleared = new EventEmitter<void>();

  filterForm: FormGroup;

  readonly UI_MESSAGES = {
    applyFilters: 'Apply Filters',
    clear: 'Clear',
    allTypes: 'All Types',
    allUsers: 'All Users',
    allStatuses: 'All Statuses',
  };

  readonly FORM_LABELS = {
    startDate: 'Start Date',
    endDate: 'End Date',
    resourceType: 'Resource Type',
    user: 'User',
    status: 'Status',
  };

  constructor(private fb: FormBuilder) {
    this.filterForm = this.fb.group({
      startDate: [''],
      endDate: [''],
      resourceType: [''],
      user: [''],
      status: [''],
    });
  }

  ngOnInit() {}

  onApplyFilters() {
    const filterValues = this.filterForm.value;
    const params: any = {};

    // Map form values to API parameters
    if (filterValues.startDate) {
      params.from = BookingHelpers.formatDateForApi(filterValues.startDate);
    }

    if (filterValues.endDate) {
      params.to = BookingHelpers.formatDateForApi(filterValues.endDate);
    }

    if (filterValues.resourceType) {
      params.resourceTypeId = parseInt(filterValues.resourceType);
    }

    if (filterValues.status) {
      params.bookingStatusId = parseInt(filterValues.status);
    }

    // User filtering - send userId to server
    if (filterValues.user) {
      params.userId = filterValues.user;
    }

    this.filtersApplied.emit(params);
  }

  onClearFilters() {
    this.filterForm.reset();
    this.filtersCleared.emit();
  }
}
