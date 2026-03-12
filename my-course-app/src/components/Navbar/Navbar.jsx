import { useNavigate, Link,  } from "react-router-dom";
import "./Navbar.css";

export default function Navbar(){
    const nav = useNavigate();
    const isAuthenticated = !!localStorage.getItem('token');
    const handleLogout = () => {
        localStorage.removeItem('token');
        nav('/login');
        window.location.reload();
    }
    return(
    <nav className="main-nav">
        <Link to="/" className="logo">CodingCourses</Link>
      
        <div className="nav-buttons">
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
    )
}
