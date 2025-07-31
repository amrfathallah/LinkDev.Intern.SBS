import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

// Angular Material Modules
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatChipsModule } from '@angular/material/chips';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSnackBarModule } from '@angular/material/snack-bar';

// Routing
import { UserBookingsComponent } from '../components/user-bookings.component/user-bookings.component';
import { UserBookingDetailsComponent } from '../components/user-booking-details.component/user-booking-details.component';
import { BookingsService } from '../services/bookings-service';
import { UserBookingsRoutingModule } from './my-bookings-routing.module';

@NgModule({
  declarations: [UserBookingsComponent, UserBookingDetailsComponent],
  imports: [
    CommonModule,
    FormsModule,
    UserBookingsRoutingModule,

    // Material Modules
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatChipsModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSnackBarModule,
  ],
  providers: [BookingsService],
  exports: [UserBookingsComponent, UserBookingDetailsComponent],
})
export class UserBookingsModule {}
