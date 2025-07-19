import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ConfirmationDialogComponent, ResourceDialogComponent } from '../shared';
import { AdminService } from '../../services/admin-service';
import { CreateResourceDto } from 'src/app/models/resource/create-resource.dto';
export interface Resource {
  id: string; // Changed from number to string to match API
  name: string;
  type: number;
  capacity: number;
  active: boolean;
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

  constructor(public dialog: MatDialog,
              private snackBar: MatSnackBar,
              private adminService: AdminService) {}

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
        const currentData = this.resourcesDataSource.data;
        const index = currentData.findIndex(r => r.id === result.id);
        if (index !== -1) {
          currentData[index] = { ...result };
          this.resourcesDataSource.data = [...currentData];

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
        resource.active = !resource.active;

        this.resourcesDataSource.data = [...this.resourcesDataSource.data];

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
        const currentData = this.resourcesDataSource.data;
        const updatedData = currentData.filter(r => r.id !== resource.id);
        this.resourcesDataSource.data = updatedData;

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
  }

  addResource() {
    const dialogRef = this.dialog.open(ResourceDialogComponent, {
      maxWidth: '500px',
      data: {
        isEdit: false
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        const currentData = this.resourcesDataSource.data;
        const createResourceDto : CreateResourceDto = {
          name: result.name,
          typeId: result.type,
          capacity: result.capacity,
          openAt: result.openAt,
          closeAt: result.closeAt
        };
        this.adminService.createResource(createResourceDto).subscribe({
          next: (createdResource: any) => {
            const newResource: Resource = {
              id: createdResource.id,
              name: createdResource.name,
              type: createdResource.typeId,
              capacity: createdResource.capacity,
              active: createdResource.isActive
            }

            this.resourcesDataSource.data = [...currentData, newResource];

            this.snackBar.open(`Resource "${newResource.name}" has been created`, 'Close', {
              duration: 3000,
              horizontalPosition: 'right',
              verticalPosition: 'top'
            });
          },
          error: (error) => {
            console.error('Error creating resource:', error);
            this.snackBar.open('Error creating resource', 'Close', {
              duration: 3000,
              horizontalPosition: 'right',
              verticalPosition: 'top'
            });
          }
        });
      }
    });
  }

  private loadResourcesData() {
    this.adminService.getResources().subscribe({
      next: (resources) => {
        // Map the API response to your local Resource interface
        const mappedResources: Resource[] = resources.map(r => ({
          id: r.id,
          name: r.name,
          type: r.typeId,
          capacity: r.capacity,
          active: r.isActive,
        }));
        this.resourcesDataSource.data = mappedResources;
      },
      error: (error) => {
        console.error('Error loading resources:', error);
        this.snackBar.open('Error loading resources', 'Close', {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top'
        });
      }
    });
  }
}
