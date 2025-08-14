import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

export interface ReportRequest {
  prompt: string;
  chart_type: string;
  x_column_hint?: string | null;
  y_column_hint?: string | null;
  top_k: number;
}

export interface ChartDataset {
  label: string;
  data: any[];
  backgroundColor?: string | string[];
  borderColor?: string | string[];
  borderWidth?: number;
}

export interface ChartData {
  type: string;
  labels: string[];
  datasets: ChartDataset[];
}

export interface ReportResponse {
  chart: ChartData;
  rows_count: number;
  sql?: string;
}

export interface ErrorResponse {
  error: string;
  sql?: string;
}

@Injectable({
  providedIn: 'root',
})
export class ReportAgentService {
  private readonly apiUrl = environment.reportAgentUrl;

  constructor(private http: HttpClient) {}

  /**
   * Generate a report using AI based on natural language prompt
   * @param request The report request parameters
   * @returns Observable<ReportResponse>
   */
  generateReport(request: ReportRequest): Observable<ReportResponse> {
    return this.http
      .post<ReportResponse>(`${this.apiUrl}/generate`, request)
      .pipe(
        map((response) => {
          // Ensure response has the expected structure
          if (
            !response.chart ||
            !response.chart.labels ||
            !response.chart.datasets
          ) {
            throw new Error('Invalid response format from server');
          }
          return response;
        }),
        catchError(this.handleError)
      );
  }

  /**
   * Test the connection to the SQL Agent API
   * @returns Observable<boolean>
   */
  testConnection(): Observable<boolean> {
    return this.http.get(`${this.apiUrl}/health`, { observe: 'response' }).pipe(
      map((response) => response.status === 200),
      catchError((error) => {
        console.warn('SQL Agent health check failed:', error);
        return throwError(
          () => new Error('SQL Agent service is not available')
        );
      })
    );
  }

  /**
   * Handle HTTP errors
   * @param error The HTTP error response
   * @returns Observable<never>
   */
  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An unknown error occurred';

    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server-side error
      if (error.error && error.error.error) {
        errorMessage = error.error.error;
      } else if (error.status === 0) {
        errorMessage =
          'Unable to connect to the SQL Agent service. Please ensure it is running on http://localhost:5000';
      } else {
        errorMessage = `Server returned code ${error.status}: ${error.message}`;
      }
    }

    console.error('ReportAgentService Error:', error);
    return throwError(() => new Error(errorMessage));
  }
}
