import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-reports-management',
  templateUrl: './reports-management.component.html',
  styleUrls: ['./reports-management.component.css']
})

// TODO
export class ReportsManagementComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {

  }

  generateReport(type: string) {
    console.log('Generating report:', type);
  }

  exportReport(format: string) {
    console.log('Exporting report as:', format);
  }
}
