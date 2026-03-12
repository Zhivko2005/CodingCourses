const API_BASE = "https://localhost:5000";

export const authService = {
    async register(userData) {
        const response = await fetch(`${API_BASE}/api/auth/register`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json', "Accept": "*/*" },
            body: JSON.stringify(userData)
        });
        if (!response.ok) {
            throw new Error("Registration failed");
        }
        return response.json();
    },
    async login(credentials) {
        const response = await fetch(`${API_BASE}/api/auth/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(credentials)
        });
        if (!response.ok) {
            throw new Error("Login failed");
        }
        return response.json();
    }
}
export const courseService = {
    async getAllCourses(){
        const response = await fetch(`${API_BASE}/api/courses`);
        if (!response.ok) {
            throw new Error("Failed to fetch courses");
        }        
        return response.json();
    }
}