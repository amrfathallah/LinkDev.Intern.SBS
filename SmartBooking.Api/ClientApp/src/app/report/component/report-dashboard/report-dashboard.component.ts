import { Component, OnInit } from '@angular/core';
import { ChartOptions, ChartType } from 'chart.js';
import { ReportService } from '../../service/report.service'; 
import { ReportDto } from '../../models/report.Dto';
import { ReportTypeEnum } from '../../models/report-type.enum'; 
import { ExportType } from '../../models/export-type.enum';

@Component({
  selector: 'app-report-dashboard',
  templateUrl: './report-dashboard.component.html',
  styleUrls: ['./report-dashboard.component.css']
})
export class ReportDashboardComponent implements OnInit {
  reportTypes = Object.keys(ReportTypeEnum)
      .filter(k => isNaN(Number(k))) as (keyof typeof ReportTypeEnum)[];
  selectedReportType: ReportTypeEnum = ReportTypeEnum.ResourceUsage;

  fromDate?: Date;
  toDate?: Date;
  reportData?: ReportDto;
  ReportTypeEnum = ReportTypeEnum;
  ExportType = ExportType;

  chartLabels: string[] = [];
  chartData: number[] = [];
  chartType: ChartType = 'bar';

  chartOptions: ChartOptions = {
    responsive: true,
    plugins: {
      legend: {
        display: false
      }
    },
    scales: {
      y:{
        ticks: {
          precision : 0
        }
      }
    }
  };

  loading = false;
  error = '';

  constructor(private reportService: ReportService) { }

  ngOnInit(): void {
    this.loadReport();
  }

  loadReport(): void {
  if (!this.fromDate || !this.toDate) {
    this.error = 'Please select both start and end dates before loading the report.';
    return;
  }

  this.loading = true;
  this.error = '';

  this.reportService.getReport(this.selectedReportType, this.fromDate, this.toDate)
    .subscribe({
      next: (data) => {
        this.reportData = data;
        this.chartLabels = data.labels;
        this.chartData = data.values;
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load report.';
        this.loading = false;
      }
    });
}

  export(type: ExportType): void {
    this.reportService.exportReport(this.selectedReportType, type, this.fromDate, this.toDate)
      .subscribe({
        next: (response) => {
          const a = document.createElement('a');
          a.href = response.fileObjectUrl;
          a.download = response.fileName;
          document.body.appendChild(a);
          a.click();
          document.body.removeChild(a);
        },
        error: () => alert('Export failed.')
      });
  }
}
