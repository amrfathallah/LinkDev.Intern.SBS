import { ReportTypeEnum } from "./report-type.enum";

export interface ReportDto{
    name: string;
    reportType: ReportTypeEnum;
    labels : string[];
    values : number[];
}