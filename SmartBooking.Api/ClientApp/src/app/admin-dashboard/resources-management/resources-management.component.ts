import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ConfirmationDialogComponent, ResourceDialogComponent } from '../shared';

export interface Resource {
  id: number;
  name: string;
  type: 'room' | 'desk';
  location: string;
  capacity: number;
  equipment: string[];
  active: boolean;
  utilizationRate: number;
  nextBooking?: string;
}

@Component({
  selector: 'app-resources-management',
  templateUrl: './resources-management.component.html',
  styleUrls: ['./resources-management.component.css']
})
export class ResourcesManagementComponent implements OnInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  resourcesDisplayedColumns: string[] = ['name', 'type', 'location', 'capacity', 'status', 'actions'];
  resourcesDataSource = new MatTableDataSource<Resource>();
  resourceType: 'all' | 'room' | 'desk' = 'all';

  constructor(public dialog: MatDialog, private snackBar: MatSnackBar) {}

  ngOnInit() {
    this.loadResourcesData();
  }

  ngAfterViewInit() {
    this.resourcesDataSource.paginator = this.paginator;
    this.resourcesDataSource.sort = this.sort;
  }

  filterResourcesByType(type: 'all' | 'room' | 'desk') {
    this.resourceType = type;
    if (type === 'all') {
      this.resourcesDataSource.filter = '';
    } else {
      this.resourcesDataSource.filter = type;
    }
  }

  editResource(resource: Resource) {
    const dialogRef = this.dialog.open(ResourceDialogComponent, {
      width: '500px',
      data: {
        resource: resource,
        isEdit: true
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        // Update the resource in the data source
        const currentData = this.resourcesDataSource.data;
        const index = currentData.findIndex(r => r.id === result.id);
        if (index !== -1) {
          currentData[index] = { ...result };
          this.resourcesDataSource.data = [...currentData];

          // Show success message
          this.snackBar.open(`Resource "${result.name}" has been updated`, 'Close', {
            duration: 3000,
            horizontalPosition: 'right',
            verticalPosition: 'top'
          });
        }
      }
    });
  }

  toggleResourceStatus(resource: Resource) {
    const action = resource.active ? 'deactivate' : 'activate';
    const actionText = resource.active ? 'Deactivate' : 'Activate';

    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      width: '400px',
      data: {
        title: `${actionText} Resource`,
        message: `Are you sure you want to ${action} "${resource.name}"?`,
        confirmText: actionText,
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        // Toggle the resource status
        resource.active = !resource.active;

        // Update the data source to trigger change detection
        this.resourcesDataSource.data = [...this.resourcesDataSource.data];

        // Show success message
        const statusText = resource.active ? 'activated' : 'deactivated';
        this.snackBar.open(`Resource "${resource.name}" has been ${statusText}`, 'Close', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top'
        });
      }
    });
  }

  deleteResource(resource: Resource) {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      width: '400px',
      data: {
        title: 'Delete Resource',
        message: `Are you sure you want to permanently delete "${resource.name}"? This action cannot be undone.`,
        confirmText: 'Delete',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        // Remove the resource from the data source
        const currentData = this.resourcesDataSource.data;
        const updatedData = currentData.filter(r => r.id !== resource.id);
        this.resourcesDataSource.data = updatedData;

        // Show success message
        this.snackBar.open(`Resource "${resource.name}" has been deleted`, 'Close', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top'
        });
      }
    });
  }

  viewResource(resource: Resource) {
    console.log('View resource:', resource);
    // Implement view resource dialog
  }

  addResource() {
    const dialogRef = this.dialog.open(ResourceDialogComponent, {
      width: '500px',
      data: {
        isEdit: false
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        // Generate a new ID for the resource
        const currentData = this.resourcesDataSource.data;
        const maxId = Math.max(...currentData.map(r => r.id), 0);
        const newResource: Resource = {
          ...result,
          id: maxId + 1
        };

        // Add the new resource to the data source
        this.resourcesDataSource.data = [...currentData, newResource];

        // Show success message
        this.snackBar.open(`Resource "${newResource.name}" has been created`, 'Close', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top'
        });
      }
    });
  }

  private loadResourcesData() {
    // Mock data - replace with actual API call
    const mockResources: Resource[] = [
      {
        id: 1,
        name: 'Conference Room A',
        type: 'room',
        location: 'Floor 1, Wing A',
        capacity: 10,
        equipment: ['Projector', 'Whiteboard', 'Video Conference'],
        active: true,
        utilizationRate: 78,
        nextBooking: '9:00 AM - John Doe'
      },
      {
        id: 2,
        name: 'Meeting Room B',
        type: 'room',
        location: 'Floor 2, Wing B',
        capacity: 6,
        equipment: ['TV Screen', 'Conference Phone'],
        active: true,
        utilizationRate: 65,
        nextBooking: '2:00 PM - Team Meeting'
      },
      {
        id: 3,
        name: 'Desk 12',
        type: 'desk',
        location: 'Floor 3, Section C',
        capacity: 1,
        equipment: ['Monitor', 'Dock Station'],
        active: true,
        utilizationRate: 45
      },
      {
        id: 4,
        name: 'Conference Room C',
        type: 'room',
        location: 'Floor 1, Wing C',
        capacity: 20,
        equipment: ['Large Screen', 'Audio System', 'Video Conference'],
        active: false,
        utilizationRate: 0,
        nextBooking: 'Under Maintenance'
      }
    ];
    this.resourcesDataSource.data = mockResources;
  }
}
