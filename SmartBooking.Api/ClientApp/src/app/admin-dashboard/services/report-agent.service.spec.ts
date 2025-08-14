import { TestBed } from '@angular/core/testing';
import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import {
  ReportAgentService,
  ReportRequest,
  ReportResponse,
} from './report-agent.service';

describe('ReportAgentService', () => {
  let service: ReportAgentService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ReportAgentService],
    });
    service = TestBed.inject(ReportAgentService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should test connection successfully', () => {
    service.testConnection().subscribe((result) => {
      expect(result).toBe(true);
    });

    const req = httpMock.expectOne('http://localhost:5000/api/reports/health');
    expect(req.request.method).toBe('GET');
    req.flush({}, { status: 200, statusText: 'OK' });
  });

  it('should generate report successfully', () => {
    const mockRequest: ReportRequest = {
      prompt: 'Test report',
      chart_type: 'bar',
      top_k: 10,
    };

    const mockResponse: ReportResponse = {
      chart: {
        type: 'bar',
        labels: ['Label1', 'Label2'],
        datasets: [{ label: 'Test', data: [1, 2] }],
      },
      rows_count: 2,
    };

    service.generateReport(mockRequest).subscribe((response) => {
      expect(response).toEqual(mockResponse);
    });

    const req = httpMock.expectOne(
      'http://localhost:5000/api/reports/generate'
    );
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(mockRequest);
    req.flush(mockResponse);
  });

  it('should handle error gracefully', () => {
    const mockRequest: ReportRequest = {
      prompt: 'Test report',
      chart_type: 'bar',
      top_k: 10,
    };

    service.generateReport(mockRequest).subscribe({
      next: () => fail('Should have failed'),
      error: (error) => {
        expect(error.message).toContain('Server returned code 500');
      },
    });

    const req = httpMock.expectOne(
      'http://localhost:5000/api/reports/generate'
    );
    req.flush(
      { error: 'Server error' },
      { status: 500, statusText: 'Internal Server Error' }
    );
  });
});
