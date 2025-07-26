import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { MainLayoutComponent } from './layouts/main-layout/main-layout.component';
import { AuthLayoutComponent } from './layouts/auth-layout/auth-layout.component';

const routes: Routes = [
  {
    path: 'auth',
    component: AuthLayoutComponent,
    loadChildren: () => import('./auth/auth.module').then((m) => m.AuthModule),
  },
  {
    path: 'admin-dashboard',
    loadChildren: () => import('./admin-dashboard/modules/admin-dashboard.module').then((m) => m.AdminDashboardModule),
  },
   {
    path: '',
    component: MainLayoutComponent,
  },


  // {
  //   path: '' , redirectTo: '/home', pathMatch: 'full',

  // },





];

@NgModule({
  declarations: [],
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
