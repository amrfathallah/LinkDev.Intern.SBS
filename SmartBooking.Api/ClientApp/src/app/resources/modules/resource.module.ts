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

// Components
import { UserResourcesComponent } from '../user-resources.component';
import { ResourceDetailsComponent } from '../components/resource-page/resource-page.component';

// Services
import { ResourceService } from '../services/resource-service';

// Routing
import { ResourceRoutingModule } from './resource-routing.module';

@NgModule({
  declarations: [
    UserResourcesComponent,
    ResourceDetailsComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ResourceRoutingModule,

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
  providers: [
    ResourceService
  ],
  exports: [
    UserResourcesComponent,
    ResourceDetailsComponent
  ]
})
export class ResourceModule {}
