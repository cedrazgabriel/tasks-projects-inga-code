import { AxiosResponse } from 'axios';
import apiClient from '../../apiClient';
import { GetTasksResponse } from './types';
import { Task } from './types';
import { UpdateTaskRequest } from './types';

export const getTasks = (page: number, pageSize: number = 10): Promise<AxiosResponse<GetTasksResponse>> => {
    return apiClient.get<GetTasksResponse>('/task', {
        params: {
            page: page,
            pageSize: pageSize
        }
    });
};

export const deleteTask = (taskId: string): Promise<AxiosResponse<void>> => {
    return apiClient.delete<void>(`/task/${taskId}`);
}

export const getTaskById = (taskId: string): Promise<AxiosResponse<Task>> => {

    return apiClient.get<Task>(`/task/${taskId}`);
}

export const updateTask = (taskId: string, request: UpdateTaskRequest): Promise<AxiosResponse<Task>> => {
    return apiClient.put<Task>(`/task/${taskId}`, request);
}

export const createTask = (request: UpdateTaskRequest): Promise<AxiosResponse<Task>> => {
    return apiClient.post<Task>('/task', request);
}



