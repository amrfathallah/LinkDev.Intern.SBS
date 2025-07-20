import { Component } from '@angular/core';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css']
})
export class AdminDashboardComponent {
  currentView: 'resources' | 'reports' = 'resources';

  switchView(view: 'resources' | 'reports') {
    this.currentView = view;
  }
}

