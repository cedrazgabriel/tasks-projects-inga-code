
import { AxiosResponse } from 'axios';
import { LoginRequest, LoginResponse } from './types';
import apiClient from '../../apiClient';

export const login = (data: LoginRequest): Promise<AxiosResponse<LoginResponse>> => {
    return apiClient.post<LoginResponse>('/user/login', data);
};


