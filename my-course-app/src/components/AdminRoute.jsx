import { Navigate } from 'react-router-dom';
import { jwtDecode } from 'jwt-decode';

const AdminRoute = ({ children }) => {
    
    const token = localStorage.getItem('token');

    if (!token) {
        return <Navigate to="/login" />;
    }

    try {
        const decodedToken = jwtDecode(token);
        
        const roles = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
        const isAdmin = Array.isArray(roles) ? roles.includes('Admin') : roles === 'Admin';


        return isAdmin ? children : <Navigate to="/" />;
    } catch (error) {
        return <Navigate to="/login" />;
    }
};
export default AdminRoute;