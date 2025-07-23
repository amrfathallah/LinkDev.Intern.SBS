import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

// Angular Material Modules
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatButtonToggleModule } from '@angular/material/button-toggle';

// Components (these would be moved from current structure)
import { ResourcesManagementComponent } from '../components/resources-management/resources-management.component';
import { ResourceDialogComponent } from '../components/resource-dialog/resource-dialog.component';

// Services
import { AdminService } from '../services/admin-service';

const routes: Routes = [
  {
    path: '',
    component: ResourcesManagementComponent
  }
];

@NgModule({
  declarations: [
    // Components would be moved here in a full refactor
    // ResourcesManagementComponent,
    // ResourceDialogComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    ReactiveFormsModule,
    FormsModule,

    // Material Modules
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatCheckboxModule,
    MatSnackBarModule,
    MatCardModule,
    MatChipsModule,
    MatButtonToggleModule
  ],
  providers: [
    AdminService
  ],
  exports: [
    // Export components if they need to be used outside this module
  ]
})
export class ResourceManagementModule { }
