import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AdminService } from '../../services/admin-service';
import { CreateResourceDto } from 'src/app/admin-dashboard/models/Dtos/create-resource.dto';
import { UpdateResourceDto } from 'src/app/admin-dashboard/models/Dtos/update-resource.dto';
import { ConfirmationDialogComponent } from '../confirmation-dialog/confirmation-dialog.component';
import { ResourceDialogComponent } from '../resource-dialog/resource-dialog.component';
import { Resource } from '../../models/Resource.model';
import { ResourceType } from '../../enums/ResourceType.enum';
import { ResourceFilter } from '../../enums/ResourceFilter.enum';


@Component({
  selector: 'app-resources-management',
  templateUrl: './resources-management.component.html',
  styleUrls: ['./resources-management.component.css']
})
export class ResourcesManagementComponent implements OnInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  // TODO: is this the best way to do it?
  // angular material requires a list of displayed columns
  // sometimes there are columns like 'actions' that are not in the model
  resourcesDisplayedColumns: string[] = ['name', 'type', 'capacity', 'openAt', 'closeAt', 'status', 'actions'];
  resourcesDataSource = new MatTableDataSource<Resource>();
  currentFilter: ResourceFilter = ResourceFilter.All;
  ResourceType = ResourceType;
  ResourceFilter = ResourceFilter;

  constructor(public dialog: MatDialog,
              private snackBar: MatSnackBar,
              private adminService: AdminService) {}

  ngOnInit() {
    this.resourcesDataSource.filterPredicate = (data: Resource, filter: string) => {
      const resourceFilter = filter as ResourceFilter;
      switch (resourceFilter) {
        case ResourceFilter.All:
          return true;
        case ResourceFilter.Desk:
          return data.typeId === ResourceType.Desk;
        case ResourceFilter.Room:
          return data.typeId === ResourceType.Room;
        default:
          return true;
      }
    };

    this.loadResourcesData();
  }

  ngAfterViewInit() {
    this.resourcesDataSource.paginator = this.paginator;
    this.resourcesDataSource.sort = this.sort;
  }

  filterResourcesByType(type: ResourceFilter) {
    this.currentFilter = type;
    this.resourcesDataSource.filter = type;
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
        const updateResourceDto: UpdateResourceDto = {
          name: result.name,
          capacity: result.capacity,
          openAt: result.openAt,
          closeAt: result.closeAt,
          isActive: result.active
        };
        this.adminService.updateResource(result.id, updateResourceDto).subscribe({
          next: (updatedResource: any) => {
            const updatedResourceData: Resource = {
              id: updatedResource.id,
              name: updatedResource.name,
              type: updatedResource.typeName,
              typeId: updatedResource.typeId,
              capacity: updatedResource.capacity,
              active: updatedResource.isActive,
              openAt: updatedResource.openAt,
              closeAt: updatedResource.closeAt
            };
            const index = currentData.findIndex(r => r.id === updatedResourceData.id);
            if (index !== -1) {
              currentData[index] = updatedResourceData;
              this.resourcesDataSource.data = [...currentData];
            }
            this.showSnackBar(`Resource "${updatedResourceData.name}" has been updated`);
          },
          error: (error) => {
            console.error('Error updating resource:', error);
            this.showSnackBar('Error updating resource', true);
          }
        });
      }
    });
  }

  deleteResource(resource: Resource) {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      maxWidth: '400px',
      data: {
        title: 'Delete Resource',
        message: `Are you sure you want to permanently delete "${resource.name}"? This action cannot be undone.`,
        confirmText: 'Delete',
        cancelText: 'Cancel'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.adminService.deleteResource(resource.id).subscribe({
          next: () => {
        this.removeResourceFromDataSource(resource);
        this.showSnackBar(`Resource "${resource.name}" has been deleted`);
          },
          error: (error) => {
            let errorMessage = 'Error deleting resource';
            if (error?.status === 400) {
              errorMessage = `Resource "${resource.name}" cannot be deleted because it has bookings.`;
            } else if (error?.status === 404) {
              errorMessage = `Resource "${resource.name}" not found.`;
            }
            console.error('Error deleting resource:', error);
            this.showSnackBar(errorMessage, true);
          }
        });
      }
    });
  }

  private removeResourceFromDataSource(resource: Resource) {
    const currentData = this.resourcesDataSource.data;
    const updatedData = currentData.filter(r => r.id !== resource.id);
    this.resourcesDataSource.data = updatedData;
  }

  private showSnackBar(message: string, isError: boolean = false) {
    this.snackBar.open(message, 'Close', {
      duration: 3000,
      horizontalPosition: 'right',
      verticalPosition: 'top',
      panelClass: isError ? 'error-snackbar' : 'success-snackbar'
    });
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
              type: createdResource.typeName,
              typeId: createdResource.typeId,
              capacity: createdResource.capacity,
              active: createdResource.isActive,
              openAt: createdResource.openAt,
              closeAt: createdResource.closeAt
            }

            this.resourcesDataSource.data = [...currentData, newResource];

            this.showSnackBar(`Resource "${newResource.name}" has been created`);
          },
          error: (error) => {
            console.error('Error creating resource:', error);
            this.showSnackBar('Error creating resource', true);
          }
        });
      }
    });
  }

  private loadResourcesData() {
    this.adminService.getResources().subscribe({
      next: (resources) => {
        const mappedResources: Resource[] = resources.map(r => ({
          id: r.id,
          name: r.name,
          type: r.typeName,
          typeId: r.typeId,
          capacity: r.capacity,
          active: r.isActive,
          openAt: r.openAt,
          closeAt: r.closeAt
        }));
        this.resourcesDataSource.data = mappedResources;
      },
      error: (error) => {
        console.error('Error loading resources:', error);
        this.showSnackBar('Error loading resources', true);
      }
    });
  }
}
