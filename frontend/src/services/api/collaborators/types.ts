export interface Collaborator {
    id: string;
    name: string;
}

export interface getCollaboratorsResponse {
    data: Collaborator[];
}