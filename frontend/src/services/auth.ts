export const AuthService = {
    isAuthenticated(): boolean {
        return !!localStorage.getItem('jwt-token');
    },

    login(token: string) {
        localStorage.setItem('jwt-token', token);
    },

    logout() {
        localStorage.removeItem('jwt-token');
    },
};
