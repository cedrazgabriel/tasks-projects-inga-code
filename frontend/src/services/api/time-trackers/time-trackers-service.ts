import { AxiosResponse } from "axios";
import apiClient from "../../apiClient";
import { GetTimeTrackerByTaskIdResponse, TimeTrackerInitRequest } from "./types";

export interface GetTimeTrackerByTaskIdParams {
    page: number;
    pageSize: number;
    collaboratorId: string;
}

export const initTimeTracker = (payload: TimeTrackerInitRequest): Promise<AxiosResponse<void>> => {
    return apiClient.post<void>(`/timetracker`, payload);
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