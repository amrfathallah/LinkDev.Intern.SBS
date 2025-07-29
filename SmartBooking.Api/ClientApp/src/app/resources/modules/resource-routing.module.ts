import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserResourcesComponent } from '../user-resources.component';
import { ResourceDetailsComponent } from '../components/resource-page/resource-page.component';

const routes: Routes = [
  {
    path: '',
    component: UserResourcesComponent,
  },
  {
    path: 'details/:id/:date',
    component: ResourceDetailsComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ResourceRoutingModule {}
