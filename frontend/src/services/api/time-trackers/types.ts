export interface TimeTrackerInitRequest {
    timeZoneId: string;
    taskId: string;
}

export interface TimeTrackerStopRequest {
    endTime: string;
}

export interface TimeTracker {
    id: string;
    createdAt: string;
    collaboratorId: string;
    collaboratorName: string;
    startDate: string;
    endDate: string;
    taskId: string;
    taskName: string;
    updatedAt: string;
    projectName: string;

}
export interface GetTimeTrackerByTaskIdResponse {
    page: number;
    pageSize: number;
    totalRecords: number;
    items: TimeTracker[];
}

export interface MetricsResponse {
    totalHoursSpentThisMonth: string;
    totalHoursSpentToday: string;
    totalHoursSpentThisWeek: string;
}
