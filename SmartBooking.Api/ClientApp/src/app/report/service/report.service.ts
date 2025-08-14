import { HttpClient, HttpHeaders, HttpParams, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ReportTypeEnum } from '../models/report-type.enum';
import { ReportDto } from '../models/report.Dto';
import { ExportTypeEnum } from '../models/export-type.enum';
import { ExportReportDto } from '../models/export-report-dto';
import { ReportRequestDto } from '../models/report-request-dto';

@Injectable({
  providedIn: 'root'
})
export class ReportService {

  constructor(private http : HttpClient) { }

    private baseUrl = `${environment.apiBaseUrl}/Reports`;


    getReport(reportRequest?: ReportRequestDto ){
      return this.http.post<ReportDto>(`${this.baseUrl}/report`, reportRequest);
    }

    exportReport(reportRequest?: ReportRequestDto){
      return this.http.post<ExportReportDto>(`${this.baseUrl}/export`, reportRequest); 
    }
}
