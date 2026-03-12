import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { authService } from '../../services/api';
import './LoginPage.css';

export default function LoginPage() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    const userLoginData = {
      email:email,
      password: password
    }
    try {
      const data = await authService.login(userLoginData);
 
      localStorage.setItem('token', data.token);
      alert('Влизането е успешно!');
      navigate('/');  
    } catch (err) {
      alert(err.message);
    }
  };

  return (
    <div className="auth-wrapper">
      <form className="auth-card" onSubmit={handleLogin}>
        <h2>Влез в профила си</h2>
        <div className="input-group">
          <label>Имейл</label>
          <input 
            type="email" 
            value={email} 
            onChange={(e) => setEmail(e.target.value)} 
            required 
          />
        </div>
        <div className="input-group">
          <label>Парола</label>
          <input 
            type="password" 
            value={password} 
            onChange={(e) => setPassword(e.target.value)} 
            required 
          />
        </div>
        <button type="submit" className="primary-btn">Впиши се</button>
        
        <div className="auth-footer">
          <span>Нямате акаунт?</span>
          <Link to="/register" className="link-btn">Регистрирайте се</Link>
        </div>
      </form>
    </div>
  );
}