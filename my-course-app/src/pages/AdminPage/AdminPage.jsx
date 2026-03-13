import { useState, useEffect } from "react";
import { courseService, userService } from "../../services/api";
import './AdminPage.css';

export default function AdminPage() {
    const [courses, setCourses] = useState([]);
    const [users, setUsers] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        fetchData();
    }, []);

    const fetchData = async () => {
        try {
            setLoading(true);
            const [coursesData, usersData] = await Promise.all([
                courseService.getAllCourses(),
                userService.getAllUsers()
            ]);
            setCourses(coursesData);
            setUsers(usersData);
        } catch (error) {
            console.error(error);
        } finally {
            setLoading(false);
        }
    };

    const handleAddRole = async (userId, roleId) => {
        try {
            await userService.assignRole(userId, roleId);
            await fetchData();
        } catch (err) {
            alert("Грешка при добавяне на роля.");
        }
    };

    const handleRemoveRole = async (userId, roleId) => {
        if (window.confirm("Сигурни ли сте?")) {
            try {
                await userService.deleteRole(userId, roleId);
                await fetchData();
            } catch (err) {
                alert("Грешка при премахване на роля.");
            }
        }
    };

    const handleDeleteUser = async (id, username) => {
        if (window.confirm(`Изтриване на ${username}?`)) {
            try {
                await userService.deleteUser(id);
                setUsers(users.filter(u => u.id !== id));
            } catch (err) {
                alert("Грешка при изтриване.");
            }
        }
    };

    const handleDeleteCourse = async (id) => {
        if (window.confirm("Изтриване на курса?")) {
            try {
                await courseService.deleteCourse(id);
                setCourses(courses.filter(c => c.id !== id));
            } catch (err) {
                alert("Грешка.");
            }
        }
    };

    if (loading) return <div className="admin-loading">Зареждане...</div>;

    return (
       <div className="admin-page">
            <header className="admin-header">
                <h1>Административен панел</h1>
                <p>Управление на потребители, роли и учебни курсове</p>
            </header>

            <div className="admin-grid">
                <section className="admin-card">
                    <h2>Потребители и Права</h2>
                    <div className="table-wrapper">
                        <table className="admin-table">
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>Потребител</th>
                                    <th>Роли</th>
                                    <th>Действия</th>
                                </tr>
                            </thead>
                            <tbody>
                                {users.map(user => (
                                    <tr key={user.id}>
                                        <td className="id-cell">{user.id}</td>
                                        <td>
                                            <div className="user-info">
                                                <span className="user-name">{user.username}</span>
                                                <span className="user-email">{user.email}</span>
                                            </div>
                                        </td>
                                        <td>
                                            <div className="roles-container">
                                                {user.roles?.map(roleName => {
                                                    const roleId = roleName === 'Admin' ? 1 : (roleName === 'Instructor' ? 2 : 3);
                                                    const isStudent = roleName.toLowerCase() === 'student';
                                                    return (
                                                        <span key={roleName} className={`role-badge ${roleName.toLowerCase()}`}>
                                                            {roleName}
                                                            {!isStudent && (
                                                                <button onClick={() => handleRemoveRole(user.id, roleId)} className="remove-role-btn">×</button>
                                                            )}
                                                        </span>
                                                    );
                                                })}
                                            </div>
                                        </td>
                                        <td>
                                            <div className="role-actions">
                                                <select onChange={(e) => handleAddRole(user.id, e.target.value)} value="">
                                                    <option value="" disabled>+ Роля</option>
                                                    <option value="1" disabled={user.roles?.includes('Admin')}>Admin</option>
                                                    <option value="2" disabled={user.roles?.includes('Instructor')}>Instructor</option>
                                                </select>
                                                <button onClick={() => handleDeleteUser(user.id, user.username)} className="btn-icon delete">🗑️</button>
                                            </div>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                </section>

                <section className="admin-card">
                    <h2>Управление на Курсове</h2>
                    <div className="admin-courses-list">
                        {courses.length === 0 ? <p className="no-data">Няма активни курсове.</p> : 
                            courses.map(course => (
                                <div key={course.id} className="admin-course-item">
                                    <div className="course-info">
                                        <span className="course-title">{course.title}</span>
                                        <div className="course-ids">
                                            <span className="course-id">Course ID: {course.id}</span>
                                            <span className="instructor-name">Instructor: {course.instructorName||'N/A'}</span>
                                        </div>
                                    </div>
                                    <button onClick={() => handleDeleteCourse(course.id)} className="delete-text">Премахни</button>
                                </div>
                            ))
                        }
                    </div>
                </section>
            </div>
        </div>
    );
}