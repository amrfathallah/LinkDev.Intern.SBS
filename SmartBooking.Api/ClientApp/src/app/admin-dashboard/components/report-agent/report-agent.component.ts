import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Chart, ChartConfiguration, ChartType, registerables } from 'chart.js';
import {
  ReportAgentService,
  ReportRequest,
} from '../../services/report-agent.service';
import {
  ReportFormData,
  CHART_TYPES,
  DEFAULT_CHART_COLORS,
} from '../../models/report-agent.models';
import { MatSnackBar } from '@angular/material/snack-bar';

Chart.register(...registerables);

@Component({
  selector: 'app-report-agent',
  templateUrl: './report-agent.component.html',
  styleUrls: ['./report-agent.component.css'],
})
export class ReportAgentComponent implements OnInit, OnDestroy {
  reportForm: FormGroup;
  isLoading = false;
  chart: Chart | null = null;
  chartTypes = CHART_TYPES;
  errorMessage: string = '';
  lastGeneratedData: any = null;
  currentDate = new Date();

  constructor(
    private fb: FormBuilder,
    private reportService: ReportAgentService,
    private snackBar: MatSnackBar
  ) {
    this.reportForm = this.fb.group({
      prompt: ['', [Validators.required, Validators.minLength(5)]],
      chartType: ['bar', Validators.required],
      xColumnHint: [''],
      yColumnHint: [''],
      topK: [
        50,
        [Validators.required, Validators.min(1), Validators.max(1000)],
      ],
    });
  }

  ngOnInit(): void {
    this.testConnection();
  }

  ngOnDestroy(): void {
    if (this.chart) {
      this.chart.destroy();
    }
  }

  private testConnection(): void {
    this.reportService.testConnection().subscribe({
      next: (isConnected: boolean) => {
        if (!isConnected) {
          this.showError('SQL Agent service is not responding');
        }
      },
      error: (error: Error) => {
        this.showError(`Connection failed: ${error.message}`);
      },
    });
  }

  onSubmit(): void {
    if (this.reportForm.valid) {
      this.isLoading = true;
      this.errorMessage = '';

      const formData: ReportFormData = this.reportForm.value;
      const requestData: ReportRequest = {
        prompt: formData.prompt,
        chart_type: formData.chartType,
        x_column_hint: formData.xColumnHint?.trim() || null,
        y_column_hint: formData.yColumnHint?.trim() || null,
        top_k: formData.topK,
      };

      this.reportService.generateReport(requestData).subscribe({
        next: (response) => {
          this.lastGeneratedData = response;
          this.currentDate = new Date(); // Update timestamp
          this.generateChart(response.chart);
          this.showSuccess(
            `Report generated successfully! Found ${response.rows_count} records.`
          );
          this.isLoading = false;
        },
        error: (error: Error) => {
          this.showError(error.message);
          this.isLoading = false;
        },
      });
    } else {
      this.markFormGroupTouched();
    }
  }

  private generateChart(chartData: any): void {
    // Wait for the view to update
    setTimeout(() => {
      const canvas = document.getElementById(
        'reportChart'
      ) as HTMLCanvasElement;
      if (!canvas) {
        console.error('Chart canvas not found');
        return;
      }

      // Destroy existing chart if it exists
      if (this.chart) {
        this.chart.destroy();
      }

      const ctx = canvas.getContext('2d');
      if (!ctx) {
        console.error('Could not get canvas context');
        return;
      }

      const config: ChartConfiguration = {
        type: chartData.type as ChartType,
        data: {
          labels: chartData.labels,
          datasets: chartData.datasets.map((dataset: any, index: number) => ({
            ...dataset,
            backgroundColor: this.getChartColors(
              chartData.type,
              chartData.labels.length
            ).backgroundColor,
            borderColor: this.getChartColors(
              chartData.type,
              chartData.labels.length
            ).borderColor,
            borderWidth: 2,
          })),
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          plugins: {
            title: {
              display: true,
              text: 'AI Generated Report Chart',
              font: {
                size: 16,
                weight: 'bold',
              },
            },
            legend: {
              display: true,
              position: 'top',
            },
            tooltip: {
              mode: 'index',
              intersect: false,
            },
          },
          scales:
            chartData.type === 'pie' || chartData.type === 'doughnut'
              ? {}
              : {
                  y: {
                    beginAtZero: true,
                    grid: {
                      color: 'rgba(0, 0, 0, 0.1)',
                    },
                  },
                  x: {
                    grid: {
                      color: 'rgba(0, 0, 0, 0.1)',
                    },
                  },
                },
          animation: {
            duration: 1000,
            easing: 'easeInOutQuart',
          },
        },
      };

      this.chart = new Chart(ctx, config);
    }, 100);
  }

  private getChartColors(
    chartType: string,
    dataLength: number
  ): { backgroundColor: string[]; borderColor: string[] } {
    const { backgroundColor, borderColor } = DEFAULT_CHART_COLORS;
    const isPieChart = chartType === 'pie' || chartType === 'doughnut';
    const colorsNeeded = isPieChart ? dataLength : 1;

    return {
      backgroundColor: backgroundColor.slice(0, colorsNeeded),
      borderColor: borderColor.slice(0, colorsNeeded),
    };
  }

  clearForm(): void {
    this.reportForm.reset({
      chartType: 'bar',
      topK: 50,
    });
    this.errorMessage = '';
    this.lastGeneratedData = null;
    this.currentDate = new Date();

    if (this.chart) {
      this.chart.destroy();
      this.chart = null;
    }

    this.showSuccess('Form cleared successfully');
  }

  private markFormGroupTouched(): void {
    Object.keys(this.reportForm.controls).forEach((key) => {
      this.reportForm.get(key)?.markAsTouched();
    });
  }

  private showSuccess(message: string): void {
    this.showNotification(message, 'success', 5000);
  }

  private showError(message: string): void {
    this.errorMessage = message;
    this.showNotification(message, 'error', 7000);
  }

  private showNotification(
    message: string,
    type: 'success' | 'error',
    duration: number
  ): void {
    this.snackBar.open(message, 'Close', {
      duration,
      panelClass: [`${type}-snackbar`],
    });
  }

  // Getter for easy access to form controls in template
  get f() {
    return this.reportForm.controls;
  }
}
