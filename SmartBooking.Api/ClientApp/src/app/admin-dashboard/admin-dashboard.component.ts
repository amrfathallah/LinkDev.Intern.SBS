import { Component } from '@angular/core';
import { AdminView } from './enums/AdminView.enum';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css']
})
export class AdminDashboardComponent {
  currentView: AdminView = AdminView.Resources;
  AdminView = AdminView;

  switchView(view: AdminView) {
    this.currentView = view;
  }
}

