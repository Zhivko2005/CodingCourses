import axios from 'axios';

const API_BASE = "https://localhost:5000";

const getAuthHeaders = () => {
    const token = localStorage.getItem('token');
    return {
        'Content-Type': 'application/json',
        'Authorization': token ? `Bearer ${token}` : ''
    };
};

export const authService = {
    async register(userData) {
        const response = await axios.post(`${API_BASE}/api/auth/register`, userData);
        return response.data;
    },

    async login(credentials) {
        const response = await axios.post(`${API_BASE}/api/auth/login`, credentials);
        return response.data;
    }
};

export const courseService = {
    async getAllCourses() {
        const response = await axios.get(`${API_BASE}/api/courses`);
        return response.data;
    },
    
    async getInstructorCourses() {
        const response = await axios.get(`${API_BASE}/api/courses/my-courses`, {
            headers: getAuthHeaders()
        });
        return response.data;
    },
    async getCourseById(id) {
        const response = await axios.get(`${API_BASE}/api/courses/${id}`, {
            headers: getAuthHeaders()
        });
        return response.data;
    }, 

    updateCourse: async (id, formData) => {
    return await axios.put(`/courses/${id}`, formData, {
        headers: { 'Content-Type': 'multipart/form-data' }
    });
    },
    async deleteCourse(courseId) {
        const response = await axios.delete(`${API_BASE}/api/courses/${courseId}`, {
            headers: getAuthHeaders() 
        });
        return response.data;
    },

    async createCourse(courseData) {
        const response = await axios.post(`${API_BASE}/api/courses`, courseData, {
            headers: getAuthHeaders()
        });
        return response.data;
    },

    async getCoursesByCategory(categoryId) {
        const response = await axios.get(`${API_BASE}/api/courses/category/${categoryId}`);
        return response.data;
    },
    async getAllCategories() {
    const response = await axios.get(`${API_BASE}/api/categories`); 
    return response.data;
}
};

export const userService = {
    async getAllUsers() {
        const response = await axios.get(`${API_BASE}/api/users`, {
            headers: getAuthHeaders()
        });
        return response.data;
    },

    async deleteUser(userId) {
        await axios.delete(`${API_BASE}/api/users/${userId}`, {
            headers: getAuthHeaders()
        });
        return true;
    },
    
    async assignRole(userId, roleId) {
        const response = await axios.post(`${API_BASE}/api/users/${userId}/roles/${roleId}`, {}, {
            headers: getAuthHeaders()
        });
        return response.data;
    },

    async deleteRole(userId, roleId) {
        await axios.delete(`${API_BASE}/api/users/${userId}/roles/${roleId}`, {
            headers: getAuthHeaders()
        });
        return true;
    }
};