import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { courseService } from "../../services/api";
import './HomePage.css';

export default function HomePage(){
    const[courses, setCourses] = useState([]);
    const[loading, setLoading] = useState(true);
    const navigate = useNavigate();

    const isAuthenticated = !!localStorage.getItem("token");
    useEffect(() => {
        const fetchCourses = async () => {
            try {
                const data = await courseService.getAllCourses();
                setCourses(data);
            } catch (error) {
                console.error("Error fetching courses:", error);
            }
            finally{
                setLoading(false);
            }
        }
        fetchCourses();
    },[])
    const handleGetCourse = (courseId) => {
        if(!isAuthenticated){
            navigate('/register');
        }
        else{
            navigate(`/course/${courseId}`);
        }
    }
    if (loading) return <div className="loader">Зареждане на курсове...</div>;
    
    return(
        <div className="homepage">
            <header className="hero-section">
                <h1>Научи се да програмираш от нулата</h1>
                <p>Избери своя път в технологичния свят с нашите професионални курсове.</p>
            </header>

            <div className="courses-container">
                <h2 className="section-title">Налични курсове ({courses.length})</h2>
                
                <div className="courses-grid">
                    {courses.map(course => (
                        <div key={course.id} className="course-card">
                            <div className="card-image">
                                <span>{course.title[0]}</span>
                            </div>
                            <div className="card-body">
                                <h3>{course.title}</h3>
                                <p>{course.description}</p>
                                <div className="card-footer">
                                    <span className="course-price">{course.price} лв.</span>
                                    <button 
                                        onClick={() => handleGetCourse(course.id)} 
                                        className="get-btn"
                                    >
                                        Get Course
                                    </button>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
}