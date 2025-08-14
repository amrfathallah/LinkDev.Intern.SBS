import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { MainLayoutComponent } from './layouts/main-layout/main-layout.component';
import { AuthLayoutComponent } from './layouts/auth-layout/auth-layout.component';
import { AuthGuard } from './auth.guard';
import { LoginGuard } from './login.guard';

const routes: Routes = [
  {
    path: 'auth',
    component: AuthLayoutComponent,
    loadChildren: () => import('./auth/auth.module').then((m) => m.AuthModule),
    canActivate: [LoginGuard],
  },
  {
    path: 'admin-dashboard',
    loadChildren: () => import('./admin-dashboard/modules/admin-dashboard.module').then((m) => m.AdminDashboardModule),
    canActivate: [AuthGuard],
  },
  {
    path: 'resources',
    loadChildren: () => import('./resources/modules/resource.module').then((m) => m.ResourceModule),
    canActivate: [AuthGuard],
  },
  {
   path: 'reports',
    loadChildren: () => import('./report/report.module').then(m => m.ReportModule)
  },
  {
    path: 'main',
    component: MainLayoutComponent,
  },
  {
    path: '',
    component: MainLayoutComponent,
  },
  {
    path: '**',
    redirectTo: '/'
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
