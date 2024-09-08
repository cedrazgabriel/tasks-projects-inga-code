
export interface Project {
    id: string;
    name: string;
    createdAt: string;
    updatedAt: string;
}

export interface GetProjectsResponse {
    page: number;
    pageSize: number;
    totalRecords: number;
    items: Project[];
}

export interface UpdateProjectRequest {
    name: string;
}

