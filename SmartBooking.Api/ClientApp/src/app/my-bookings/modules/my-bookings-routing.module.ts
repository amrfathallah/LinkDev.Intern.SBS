import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserBookingDetailsComponent } from '../components/user-booking-details.component/user-booking-details.component';
import { UserBookingsComponent } from '../components/user-bookings.component/user-bookings.component';

const routes: Routes = [
  {
    path: '',
    component: UserBookingsComponent,
  },
  {
    path: 'details/:id',
    component: UserBookingDetailsComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class UserBookingsRoutingModule {}
