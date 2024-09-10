import { AxiosResponse } from "axios";
import apiClient from "../../apiClient";
import { getCollaboratorsResponse } from "./types";

export const getCollaborators = (): Promise<AxiosResponse<getCollaboratorsResponse>> => {
    return apiClient.get<getCollaboratorsResponse>('/collaborator');
};