import {useState, useEffect} from 'react';
import { useNavigate } from 'react-router-dom';
import { courseService } from '../../services/api';
import './InstructorCoursesPage.css';

export default function InstructorCoursesPage(){
    const [courses, setCourses] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const navigate = useNavigate();
    useEffect(() => {
        const loadCourses = async () => {
            try {
                setLoading(true);
                const data = await courseService.getInstructorCourses();
                setCourses(data);
            } catch (error) {
                setError("Грешка при зареждане на курсовете.");
                console.error(error);
            }
            finally {
                setLoading(false);
            }
        }
        loadCourses();
    }, []);
    if (loading) {
        return <div className="loading-state">Зареждане на вашите курсове...</div>;
    }   
    if (error) {
        return <div className="error-state">{error}</div>;
    }
    return (
        <div className="my-courses-page">
            <div className="my-courses-header">
                <h1>Моите Курсове</h1>
                <p>Управлявайте съдържанието, което преподавате</p>
            </div>

            <div className="courses-list-container">
                {courses.length === 0 ? (
                    <div className="no-courses-msg">
                        <p>Все още не сте създали курсове.</p>
                        <button className="btn-primary">Създай първия си курс</button>
                    </div>
                ) : (
                    <div className="courses-grid">
                        {courses.map(course => (
                            <div key={course.id} className="instructor-course-card">
                                <div className="course-card-content">
                                    <div className="course-main-info">
                                        <h3>{course.title}</h3>
                                        <span className="course-price">{course.price} лв.</span>
                                    </div>
                                    <p className="course-desc">
                                        {course.description?.length > 120 
                                            ? `${course.description.substring(0, 120)}...` 
                                            : course.description}
                                    </p>
                                    <div className="course-tags">
                                        {course.categories?.map(cat => (
                                            <span key={cat} className="tag">{cat}</span>
                                        ))}
                                    </div>
                                </div>
                                <div className="course-card-actions">
                                    <button     
                                        className="btn-edit-course" 
                                        onClick={() => navigate(`/edit-course/${course.id}`)}
                                    >
                                        ✏️ Редактирай
                                    </button>
                                </div>
                            </div>
                        ))}
                    </div>
                )}
            </div>
        </div>
    );
}