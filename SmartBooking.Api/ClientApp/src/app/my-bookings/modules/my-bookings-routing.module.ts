import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserBookingsComponent } from '../components/user-bookings.component/user-bookings.component';

const routes: Routes = [
  {
    path: '',
    component: UserBookingsComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class UserBookingsRoutingModule {}
