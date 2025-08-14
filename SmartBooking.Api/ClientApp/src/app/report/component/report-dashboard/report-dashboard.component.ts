import { Component, OnInit } from '@angular/core';
import { ChartOptions, ChartType } from 'chart.js';
import { ReportService } from '../../service/report.service'; 
import { ReportDto } from '../../models/report.Dto';
import { ReportTypeEnum } from '../../models/report-type.enum'; 
import { ExportTypeEnum } from '../../models/export-type.enum';
import { ReportRequestDto } from '../../models/report-request-dto';

@Component({
  selector: 'app-report-dashboard',
  templateUrl: './report-dashboard.component.html',
  styleUrls: ['./report-dashboard.component.css']
})
export class ReportDashboardComponent implements OnInit {
  reportTypes = Object.keys(ReportTypeEnum)
      .filter(k => isNaN(Number(k))) as (keyof typeof ReportTypeEnum)[];
  selectedReportType: ReportTypeEnum = ReportTypeEnum.ResourceUsage;

  reportRequest: ReportRequestDto = new ReportRequestDto();

  fromDate?: Date;
  toDate?: Date;
  reportData?: ReportDto;
  ReportTypeEnum = ReportTypeEnum;
  ExportType = ExportTypeEnum;

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
  
  if(this.reportRequest){
    this.reportRequest.reportType = this.selectedReportType;
    this.reportRequest.from = this.fromDate?.toISOString().split('T')[0];
    this.reportRequest.to = this.toDate?.toISOString().split('T')[0];
  }

  this.reportService.getReport(this.reportRequest)
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

  export(type: ExportTypeEnum): void {
    if(this.reportRequest){
      this.reportRequest.exportType = type;
    }
    this.reportService.exportReport(this.reportRequest)
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
