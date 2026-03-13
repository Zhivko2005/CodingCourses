import { useNavigate, Link } from "react-router-dom";
import { jwtDecode } from "jwt-decode";  
import "./Navbar.css";

export default function Navbar() {
    const nav = useNavigate();
    const token = localStorage.getItem('token');
    const isAuthenticated = !!token;
     
    let isAdmin = false;
    if (token) {
        try {
            const decoded = jwtDecode(token); 

        const roles = decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] 
                      || decoded.role 
                      || decoded.roles;

        isAdmin = Array.isArray(roles) ? roles.includes('Admin') : roles === 'Admin';
        } catch (error) {
            console.error("Invalid token", error);
        }
    }

    const handleLogout = () => {
        localStorage.removeItem('token');
        nav('/login');
        window.location.reload();
    };

    return (
        <nav className="main-nav">
            <Link to="/" className="logo">CodingCourses</Link>
          
            <div className="nav-buttons"> 
                {isAdmin && location.pathname !== '/admin' && (
                    <button onClick={() => nav('/admin')} className="btn-admin">
                        Admin Panel
                    </button>
                )}

                {!isAuthenticated ? (
                    <>
                        <button onClick={() => nav('/login')} className="btn-login">Login</button>
                        <button onClick={() => nav('/register')} className="btn-register">Register</button>
                    </>
                ) : (
                    <button onClick={handleLogout} className="btn-logout">Logout</button>
                )}
            </div>
        </nav>
    );
}