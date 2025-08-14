export interface ReportFormData {
  prompt: string;
  chartType: string;
  xColumnHint: string;
  yColumnHint: string;
  topK: number;
}

export interface ChartColors {
  backgroundColor: string[];
  borderColor: string[];
}

export const CHART_TYPES = [
  { value: 'bar', display: 'Bar Chart', icon: 'bar_chart' },
  { value: 'line', display: 'Line Chart', icon: 'show_chart' },
  { value: 'pie', display: 'Pie Chart', icon: 'pie_chart' },
  { value: 'doughnut', display: 'Doughnut Chart', icon: 'donut_large' },
] as const;

export const DEFAULT_CHART_COLORS: ChartColors = {
  backgroundColor: [
    'rgba(54, 162, 235, 0.6)', // Blue
    'rgba(255, 99, 132, 0.6)', // Red
    'rgba(255, 205, 86, 0.6)', // Yellow
    'rgba(75, 192, 192, 0.6)', // Teal
    'rgba(153, 102, 255, 0.6)', // Purple
    'rgba(255, 159, 64, 0.6)', // Orange
    'rgba(199, 199, 199, 0.6)', // Grey
    'rgba(83, 102, 255, 0.6)', // Indigo
    'rgba(255, 0, 255, 0.6)', // Magenta
    'rgba(0, 255, 255, 0.6)', // Cyan
  ],
  borderColor: [
    'rgba(54, 162, 235, 1)',
    'rgba(255, 99, 132, 1)',
    'rgba(255, 205, 86, 1)',
    'rgba(75, 192, 192, 1)',
    'rgba(153, 102, 255, 1)',
    'rgba(255, 159, 64, 1)',
    'rgba(199, 199, 199, 1)',
    'rgba(83, 102, 255, 1)',
    'rgba(255, 0, 255, 1)',
    'rgba(0, 255, 255, 1)',
  ],
};
