import { AxiosResponse } from 'axios';
import apiClient from '../../apiClient';
import { GetProjectsResponse, Project, UpdateProjectRequest } from './types';

export const getProjects = (page: number, pageSize: number = 10): Promise<AxiosResponse<GetProjectsResponse>> => {
    return apiClient.get<GetProjectsResponse>('/project', {
        params: {
            page: page,
            pageSize: pageSize
        }
    });
};

export const deleteProject = (projectId: string): Promise<AxiosResponse<void>> => {
    return apiClient.delete<void>(`/project/${projectId}`);
}

export const getProjectById = (projectId: string): Promise<AxiosResponse<Project>> => {

    return apiClient.get<Project>(`/project/${projectId}`);
}

export const updateProject = (projectId: string, request: UpdateProjectRequest): Promise<AxiosResponse<Project>> => {
    return apiClient.put<Project>(`/project/${projectId}`, request);
}

export const createProject = (request: UpdateProjectRequest): Promise<AxiosResponse<Project>> => {
    return apiClient.post<Project>('/project', request);
}
