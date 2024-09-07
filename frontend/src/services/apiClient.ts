import axios from 'axios';


const apiClient = axios.create({
    baseURL: 'http://localhost:5231/api', // URL base da sua API
    headers: {
        'Content-Type': 'application/json',
    },
});

apiClient.interceptors.request.use((config) => {
    const token = localStorage.getItem('jwt-token');
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

export default apiClient;
