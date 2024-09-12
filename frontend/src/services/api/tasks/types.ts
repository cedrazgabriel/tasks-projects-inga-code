
export interface Task {
    id: string;
    name: string;
    description: string;
    createdAt: string;
    updatedAt: string;
    projectId: string;
    projectName: string;
    totalTimeSpent: string;
}

export interface GetTasksResponse {
    page: number;
    pageSize: number;
    totalRecords: number;
    items: Task[];
}

export interface UpdateTaskRequest {
    name: string;
    description: string;
    projectId: string;
}


