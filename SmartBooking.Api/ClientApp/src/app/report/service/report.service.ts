import { HttpClient, HttpHeaders, HttpParams, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ReportTypeEnum } from '../models/report-type.enum';
import { ReportDto } from '../models/report.Dto';
import { ExportType } from '../models/export-type.enum';
import { ExportReportDto } from '../models/export-report-dto';

@Injectable({
  providedIn: 'root'
})
export class ReportService {

  constructor(private http : HttpClient) { }

    private baseUrl = `${environment.apiBaseUrl}/Reports`;


    getReport(reportType: ReportTypeEnum, from? : Date, to? : Date ){
      var params = new HttpParams().set('reportType', reportType);
      
      if(from)
        params = params.set('from', from.toISOString().split('T')[0]);

      if(to)
        params = params.set('to', to.toISOString().split('T')[0]);

      return this.http.get<ReportDto>(`${this.baseUrl}/report`, {params});
    }

    exportReport(reportType: ReportTypeEnum, exportType: ExportType, from? : Date, to? : Date){
      var params = new HttpParams()
        .set('reportType', reportType)
        .set('exportType', exportType);

      if(from)
        params = params.set('from', from.toISOString().split('T')[0]);

      if(to)
        params = params.set('to', to.toISOString().split('T')[0]);

      return this.http.get<ExportReportDto>(`${this.baseUrl}/export`, {params}); 
    }
}
