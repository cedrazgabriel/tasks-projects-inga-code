import { AxiosResponse } from "axios";
import apiClient from "../../apiClient";
import { GetTimeTrackerByTaskIdResponse, MetricsResponse, TimeTrackerInitRequest, TimeTrackerStopRequest } from "./types";


export interface GetTimeTrackerByTaskIdParams {
    page: number;
    pageSize: number;
    collaboratorId: string | undefined;
}

export const initTimeTracker = (payload: TimeTrackerInitRequest): Promise<AxiosResponse<void>> => {
    return apiClient.post<void>(`/timetracker`, payload);
}

export const stopTimeTracker = (timeTrackerId: string, payload: TimeTrackerStopRequest): Promise<AxiosResponse<void>> => {
    return apiClient.post<void>(`/timetracker/${timeTrackerId}/stop`, payload);
}

export const getTimeTrackerByTaskId = ({ collaboratorId, page, pageSize }: GetTimeTrackerByTaskIdParams, taskId: string): Promise<AxiosResponse<GetTimeTrackerByTaskIdResponse>> => {
    return apiClient.get<GetTimeTrackerByTaskIdResponse>(`/timetracker/${taskId}`, {
        params: {
            page: page,
            pageSize: pageSize,
            collaboratorId: collaboratorId
        }
    });
};

export const getMetrics = (): Promise<AxiosResponse<MetricsResponse>> => {
    return apiClient.get<MetricsResponse>('/timetracker/metrics');
}
