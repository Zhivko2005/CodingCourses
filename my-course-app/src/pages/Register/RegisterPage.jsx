import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { authService } from "../../services/api";
import './RegisterPage.css';

export default function RegisterPage() {
  const [username, setUsername] = useState('')
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();

  const handleRegister = async (e) => {
    e.preventDefault();
    const userRegisterData = {
      username : username,
      email : email,
      password:password
    }
    try {
      await authService.register(userRegisterData);
      alert('Registration successful!. Please log in.');
      navigate('/login');
    } catch (err) {
      alert(err.message);
    }
  };

  return (
    
    <div className="auth-wrapper">
      <form className="auth-card" onSubmit={handleRegister}>
        <h2>Създай акаунт</h2>
        <div className = "input-group">
          <label>Потребителско име</label>
          <input type="username" value = {username} onChange={(e) => setUsername(e.target.value)} required/>
        </div>
        <div className="input-group">
          <label>Имейл</label>
          <input type="email" value={email} onChange={(e) => setEmail(e.target.value)} required />
        </div>
        <div className="input-group">
          <label>Парола</label>
          <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} required />
        </div>
        <button type="submit" className="primary-btn">Регистрирай се</button>
        
        <div className="auth-footer">
          <span>Вече имате акаунт?</span>
          <Link to="/login" className="link-btn">Впишете се</Link>
        </div>
      </form>
    </div>
  );
}
