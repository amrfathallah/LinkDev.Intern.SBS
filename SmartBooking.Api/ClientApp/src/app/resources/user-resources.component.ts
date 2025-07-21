import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { GetResourceDto } from '../models/resource/get-resources.dto';
import { Router } from '@angular/router';

export interface Resource {
  id: number;
  name: string;
  type: string;
  location: string;
  capacity: number;
  active: boolean;
  nextBooking?: string;
  description?: string;
}

@Component({
  selector: 'app-user-resources',
  templateUrl: './user-resources.component.html',
  styleUrls: ['./user-resources.component.css'],
})
export class UserResourcesComponent implements OnInit {
  resources: GetResourceDto[] = [];
  filteredResources: GetResourceDto[] = [];
  selectedType: 'all' | 1 | 2 = 'all';
  searchQuery: string = '';
  selectedDate: Date = new Date();

  constructor(private snackBar: MatSnackBar, private router: Router) {}

  ngOnInit() {
    this.loadResources();
  }

  private loadResources() {
    // Mock data - replace with actual API call
    const resources: GetResourceDto[] = [
      {
        id: '1',
        name: 'Conference Room A',
        typeId: 1,
        type: 'room',
        capacity: 12,
        isActive: true,
        openAt: '09:00:00',
        closeAt: '17:00:00',
      },
      {
        id: '2',
        name: 'Hot Desk 101',
        typeId: 2,
        type: 'Work Desk',
        capacity: 1,
        isActive: true,
        openAt: '08:00:00',
        closeAt: '18:00:00',
      },
      {
        id: '3',
        name: 'Private Office 1',
        typeId: 3,
        type: 'Private Office',
        capacity: 4,
        isActive: true,
        openAt: '09:30:00',
        closeAt: '19:30:00',
      },
      {
        id: '4',
        name: 'Workshop Room',
        typeId: 4,
        type: 'Training Room',
        capacity: 20,
        isActive: false,
        openAt: '10:00:00',
        closeAt: '16:00:00',
      },
      {
        id: '5',
        name: 'Huddle Room A',
        typeId: 5,
        type: 'Huddle Room',
        capacity: 6,
        isActive: true,
        openAt: '08:30:00',
        closeAt: '17:30:00',
      },
      {
        id: '6',
        name: 'Silent Zone',
        typeId: 6,
        type: 'Quiet Room',
        capacity: 5,
        isActive: false,
        openAt: '07:00:00',
        closeAt: '15:00:00',
      },
      {
        id: '7',
        name: 'Open Space A',
        typeId: 7,
        type: 'Open Area',
        capacity: 25,
        isActive: true,
        openAt: '06:00:00',
        closeAt: '22:00:00',
      },
      {
        id: '8',
        name: 'Conference Room B',
        typeId: 1,
        type: 'Meeting Room',
        capacity: 15,
        isActive: true,
        openAt: '09:00:00',
        closeAt: '18:00:00',
      },
      {
        id: '9',
        name: 'Hot Desk 102',
        typeId: 2,
        type: 'Work Desk',
        capacity: 1,
        isActive: false,
        openAt: '08:00:00',
        closeAt: '17:00:00',
      },
      {
        id: '10',
        name: 'Project Lab',
        typeId: 8,
        type: 'Lab',
        capacity: 10,
        isActive: true,
        openAt: '10:00:00',
        closeAt: '20:00:00',
      },
    ];

    this.resources = resources.filter((resource) => resource.isActive);
    this.applyFilters();
  }

  filterByType(type: 'all' | 1 | 2) {
    this.selectedType = type;
    this.applyFilters();
  }

  onSearchChange(query: string) {
    this.searchQuery = query;
    this.applyFilters();
  }

  private applyFilters() {
    let filtered = this.resources;

    // Filter by type
    if (this.selectedType !== 'all') {
      filtered = filtered.filter(
        (resource) => resource.typeId === this.selectedType
      );
    }

    // Filter by search query
    if (this.searchQuery.trim()) {
      const query = this.searchQuery.toLowerCase();
      filtered = filtered.filter((resource) =>
        resource.name.toLowerCase().includes(query)
      );
    }

    this.filteredResources = filtered;
  }

  bookResource(resourceId: string) {
    this.router.navigate([
      '/resource-details',
      resourceId,
      this.selectedDate.toISOString().split('T')[0],
    ]);
  }

  getResourceIcon(type: string): string {
    return type === 'room' ? 'meeting_room' : 'desk_chair';
  }

  getStatusColor(resource: Resource): string {
    if (resource.nextBooking?.includes('Available')) {
      return 'primary';
    } else if (resource.nextBooking?.includes('Booked')) {
      return 'warn';
    }
    return 'accent';
  }

  getUtilizationColor(rate: number): string {
    if (rate >= 70) return 'warn';
    if (rate >= 40) return 'accent';
    return 'primary';
  }
}
