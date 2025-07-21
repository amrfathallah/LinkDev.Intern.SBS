import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

// Angular Material Modules (commonly used across features)
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';

// Shared Components
import { ConfirmationDialogComponent } from '../components/confirmation-dialog/confirmation-dialog.component';

// Shared Pipes (you can create these)
// import { CustomDatePipe } from './pipes/custom-date.pipe';
// import { StatusPipe } from './pipes/status.pipe';

// Shared Directives (you can create these)
// import { HighlightDirective } from './directives/highlight.directive';

@NgModule({
  declarations: [
    // Shared components that are used across multiple modules
    // ConfirmationDialogComponent,

    // Shared pipes
    // CustomDatePipe,
    // StatusPipe,

    // Shared directives
    // HighlightDirective
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,

    // Common Material modules
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSnackBarModule,
    MatProgressSpinnerModule,
    MatTooltipModule
  ],
  exports: [
    // Export everything that other modules might need
    CommonModule,
    ReactiveFormsModule,
    FormsModule,

    // Material modules
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSnackBarModule,
    MatProgressSpinnerModule,
    MatTooltipModule,

    // Shared components
    // ConfirmationDialogComponent,

    // Shared pipes
    // CustomDatePipe,
    // StatusPipe,

    // Shared directives
    // HighlightDirective
  ]
})
export class AdminSharedModule { }
