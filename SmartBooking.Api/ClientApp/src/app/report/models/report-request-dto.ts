import { ExportTypeEnum } from "./export-type.enum";
import { ReportTypeEnum } from "./report-type.enum";

export class ReportRequestDto{
    reportType?: ReportTypeEnum;
    exportType?: ExportTypeEnum;
    from?: string;
    to ?: string;
}