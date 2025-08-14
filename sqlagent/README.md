# Report Agent - AI-Powered Reporting System

## Overview

The Report Agent is an intelligent reporting component that uses AI to generate SQL queries from natural language prompts and displays the results as interactive charts using Chart.js.

## Architecture

### Frontend (Angular)

- **ReportAgentComponent**: Main UI component with form and chart display
- **ReportAgentService**: HTTP service for API communication
- **Models**: TypeScript interfaces for type safety

### Backend (Flask)

- **Flask API**: Processes natural language prompts
- **AI Integration**: Uses LLM to generate SQL queries
- **Database**: Connects to SQL Server database
- **Chart Data**: Formats results for Chart.js

## Features

### 1. Natural Language Queries

Users can describe what they want to see in plain English:

- "Show me the most booked resources last month"
- "Display user registration trends by month"
- "Compare resource utilization across types"

### 2. Multiple Chart Types

- Bar Charts
- Line Charts
- Pie Charts
- Doughnut Charts

### 3. Advanced Options

- X/Y axis column hints
- Configurable result limits
- Real-time error handling
- Connection health monitoring

### 4. Smart Features

- Auto-color selection for charts
- Responsive design
- Form validation
- Success/error notifications
- Loading states

## API Endpoints

### POST /api/reports/generate

Generates a report from natural language prompt.

**Request:**

```json
{
  "prompt": "Show me booking counts by resource",
  "chart_type": "bar",
  "x_column_hint": "resource_name",
  "y_column_hint": "booking_count",
  "top_k": 50
}
```

**Response:**

```json
{
  "chart": {
    "type": "bar",
    "labels": ["Resource A", "Resource B"],
    "datasets": [
      {
        "label": "booking_count",
        "data": [25, 15]
      }
    ]
  },
  "rows_count": 2
}
```

### GET /api/reports/health

Health check endpoint for service monitoring.

## Database Schema

The system works with the following tables:

- AspNetUsers (Users)
- Bookings (Booking records)
- Resources (Available resources)
- BookingSlots (Time slots)
- BookingStatuses (Status types)
- ResourceTypes (Resource categories)

## Security Features

- SQL injection protection
- Query validation against allowed schema
- Row limit enforcement
- Safe query generation only (SELECT statements)

## Setup Instructions

### Prerequisites

- Node.js and Angular CLI
- Python 3.8+
- SQL Server database
- ODBC Driver 17 for SQL Server

### Backend Setup

1. Navigate to sqlagent directory
2. Create and activate virtual environment:
   ```bash
   python -m venv venv
   venv\Scripts\activate
   ```
3. Install dependencies:
   ```bash
   pip install -r requirements.txt
   ```
4. Configure .env file with database settings
5. Start Flask server:
   ```bash
   python main.py
   ```

### Frontend Setup

1. Navigate to ClientApp directory
2. Install dependencies:
   ```bash
   npm install
   npm install chart.js
   ```
3. Start Angular development server:
   ```bash
   ng serve
   ```

## Usage

1. Navigate to Admin Dashboard
2. Click on "Report Agent" tab
3. Enter a natural language description of desired report
4. Select chart type and configure options
5. Click "Generate Report"
6. View the generated chart and results summary

## Error Handling

- Connection failures are displayed with clear messages
- Invalid queries are rejected with helpful feedback
- Database errors are logged and sanitized for display
- Network timeouts are handled gracefully

## Performance Considerations

- Results are limited to prevent large data sets
- Charts are optimized for rendering performance
- Caching can be implemented for frequently used queries
- Database connection pooling for scalability

## Future Enhancements

- Export charts as images/PDF
- Save and share reports
- Scheduled report generation
- Advanced filtering options
- Chart customization options
- Multi-database support
