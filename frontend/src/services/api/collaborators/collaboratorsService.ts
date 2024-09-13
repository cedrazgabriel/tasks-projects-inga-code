import { AxiosResponse } from "axios";
import apiClient from "../../apiClient";
import { Collaborator } from "./types";

export const getCollaborators = (): Promise<AxiosResponse<Collaborator[]>> => {
    return apiClient.get<Collaborator[]>('/collaborator');
};