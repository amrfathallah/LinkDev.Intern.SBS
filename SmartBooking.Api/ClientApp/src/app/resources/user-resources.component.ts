import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { GetResourceDto } from './models/dtos/get-resources.dto';
import { Router } from '@angular/router';
import { ResourceService } from './services/resource-service';
import { ApiResponse } from '../shared/models/api-response.model';

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
    private resourceService: ResourceService
  ) {}

  ngOnInit() {
    this.loadResources();
  }

  private loadResources() {
    this.resourceService.getResources().subscribe({
      next: (data: GetResourceDto[]) => {
        this.resources = data;
        this.resources = this.resources.filter((resource) => resource.isActive);
        this.applyFilters();
      },
      error: (err: any) => {
        console.error('Failed to fetch resources', err);
        this.snackBar.open('Failed to load resources', 'Close', {
          duration: 3000,
          panelClass: ['error-snackbar']
        });
      },
    });
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
      '/resources/details',
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
}
