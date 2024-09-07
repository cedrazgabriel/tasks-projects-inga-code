import { AxiosResponse } from 'axios';
import apiClient from '../../apiClient';
import { GetProjectsResponse } from './types';

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
