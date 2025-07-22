import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { GetResourceDto } from '../models/resource/get-resources.dto';
import { Router } from '@angular/router';
import { AdminService } from '../services/admin-service';

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

  constructor(
    private snackBar: MatSnackBar,
    private router: Router,
    private adminService: AdminService
  ) {}

  ngOnInit() {
    this.loadResources();
  }

  private loadResources() {
    this.adminService.getResources().subscribe({
      next: (data: GetResourceDto[]) => {
        this.resources = data;
      },
      error: (err) => {
        console.error('Failed to fetch resources', err);
      },
    });

    this.resources = this.resources.filter((resource) => resource.isActive);
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
