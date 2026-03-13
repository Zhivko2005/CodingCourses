
import axios from 'axios';
const API_BASE = "https://localhost:5000";


const getAuthHeaders = () => ({
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${localStorage.getItem('token')}` 
});

export const authService = {
    async register(userData) {
        const response = await fetch(`${API_BASE}/api/auth/register`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(userData)
        });
        if (!response.ok) throw new Error("Registration failed");
        return response.json();
    },

    async login(credentials) {
        const response = await fetch(`${API_BASE}/api/auth/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(credentials)
        });
        if (!response.ok) throw new Error("Login failed");
        return response.json();
    }
}

export const courseService = {
    async getAllCourses() {
        const response = await fetch(`${API_BASE}/api/courses`);
        if (!response.ok) throw new Error("Failed to fetch courses");
        return response.json();
    },
    
    async deleteCourse(courseId) {
        const response = await fetch(`${API_BASE}/api/courses/${courseId}`, {
            method: 'DELETE',
            headers: getAuthHeaders() 
        });
        if (!response.ok) throw new Error("Failed to delete course");
    },
     async getInstructorCourses() {
       const token = localStorage.getItem('token');
       const response = await axios.get(`${API_BASE}/api/courses/my-courses`, {
           headers: {Authorization: `Bearer ${token}`}
       });
       return response.data;
    }
}

export const userService = {
    async getAllUsers() {
        const response = await fetch(`${API_BASE}/api/users`, {
            headers: getAuthHeaders() 
        });
        if (!response.ok) throw new Error("Failed to fetch users");
        return response.json();
    },

    async deleteUser(userId) {
        const response = await fetch(`${API_BASE}/api/users/${userId}`, {
            method: 'DELETE',
            headers: getAuthHeaders() 
        });
        if (!response.ok) throw new Error("Failed to delete user");
        return true;
    },
    
    async assignRole(userId, roleID) {
        const response = await fetch(`${API_BASE}/api/users/${userId}/roles/${roleID}`, {
            method: 'POST',
            headers: getAuthHeaders()
        });
        if (!response.ok) throw new Error("Failed to update role");
        return response.text();
    },

    async deleteRole(userId, roleId) {
        const response = await fetch(`${API_BASE}/api/users/${userId}/roles/${roleId}`, {
            method: 'DELETE',
            headers: getAuthHeaders()
        });
        if (!response.ok) throw new Error("Failed to delete role");
        return true;
    }
}