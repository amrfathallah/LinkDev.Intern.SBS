import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

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
  styleUrls: ['./user-resources.component.css']
})
export class UserResourcesComponent implements OnInit {
  resources: Resource[] = [];
  filteredResources: Resource[] = [];
  selectedType: 'all' | 'room' | 'desk' = 'all';
  searchQuery: string = '';

  constructor(
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.loadResources();
  }

  private loadResources() {
    // Mock data - replace with actual API call
    const mockResources: Resource[] = [
      {
        id: 1,
        name: 'Conference Room A',
        type: 'room',
        location: 'Floor 1, Wing A',
        capacity: 10,
        active: true,
        description: 'Modern conference room with state-of-the-art equipment for presentations and video calls.',
      },
      {
        id: 2,
        name: 'Meeting Room B',
        type: 'room',
        location: 'Floor 2, Wing B',
        capacity: 6,
        active: true,
        description: 'Intimate meeting space perfect for small team discussions and brainstorming sessions.',
      },
      {
        id: 3,
        name: 'Desk 12',
        type: 'desk',
        location: 'Floor 3, Section C',
        capacity: 1,
        active: true,
        description: 'Quiet desk space with dual monitor setup and ergonomic chair.',
      },
      {
        id: 4,
        name: 'Conference Room C',
        type: 'room',
        location: 'Floor 1, Wing C',
        capacity: 20,
        active: true,
        description: 'Large presentation room ideal for company meetings and client presentations.',
      },
      {
        id: 5,
        name: 'Desk 08',
        type: 'desk',
        location: 'Floor 2, Section A',
        capacity: 1,
        active: true,
        description: 'Premium desk space with standing desk option and city view.',
      },
      {
        id: 6,
        name: 'Creative Room',
        type: 'room',
        location: 'Floor 3, Wing A',
        capacity: 8,
        active: true,
        nextBooking: 'Available Now',
        description: 'Informal meeting space designed for creative sessions and team collaboration.',
      }
    ];

    this.resources = mockResources.filter(resource => resource.active);
    this.applyFilters();
  }

  filterByType(type: 'all' | 'room' | 'desk') {
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
      filtered = filtered.filter(resource => resource.type === this.selectedType);
    }

    // Filter by search query
    if (this.searchQuery.trim()) {
      const query = this.searchQuery.toLowerCase();
      filtered = filtered.filter(resource =>
        resource.name.toLowerCase().includes(query) ||
        resource.location.toLowerCase().includes(query)
      );
    }

    this.filteredResources = filtered;
  }

  bookResource(resource: Resource) {
    // TODO
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
