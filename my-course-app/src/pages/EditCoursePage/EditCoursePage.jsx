import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { courseService } from '../../services/api';
import './EditCoursePage.css';

export default function EditCoursePage() {
    const { id } = useParams();
    const navigate = useNavigate();
    const [loading, setLoading] = useState(true);
    
    const [courseData, setCourseData] = useState({
        title: '',
        description: '',
        price: 0,
        previewVideoUrl: '',
        previewVideoFile: null,
        previewSource: 'link', // 'link' или 'file'
        categoryIds: [],
        lessons: []
    });

    useEffect(() => {
        const loadData = async () => {
            try {
                const data = await courseService.getCourseById(id);
                setCourseData({
                    ...data,
                    previewSource: data.previewVideoUrl ? 'link' : 'file',
                    lessons: data.lessons.map(l => ({
                        ...l,
                        videoSource: l.videoUrl ? 'link' : 'file',
                        assignments: l.assignments || []
                    }))
                });
            } catch (err) { console.error(err); }
            finally { setLoading(false); }
        };
        loadData();
    }, [id]);

    const handleSubmit = async (e) => {
        e.preventDefault();
        const data = new FormData();
        
        // Основни данни
        data.append('Title', courseData.title);
        data.append('Description', courseData.description);
        data.append('Price', courseData.price);

        // Preview Video Логика
        if (courseData.previewSource === 'file' && courseData.previewVideoFile) {
            data.append('PreviewVideoFile', courseData.previewVideoFile);
        } else {
            data.append('PreviewVideoUrl', courseData.previewVideoUrl);
        }

        // Уроци и Задания
        courseData.lessons.forEach((lesson, lIdx) => {
            data.append(`Lessons[${lIdx}].LessonTitle`, lesson.lessonTitle);
            data.append(`Lessons[${lIdx}].Description`, lesson.description);
            
            if (lesson.videoSource === 'file' && lesson.videoFile) {
                data.append(`Lessons[${lIdx}].VideoFile`, lesson.videoFile);
            } else {
                data.append(`Lessons[${lIdx}].VideoUrl`, lesson.videoUrl);
            }

            lesson.assignments.forEach((assign, aIdx) => {
                data.append(`Lessons[${lIdx}].Assignments[${aIdx}].Instructions`, assign.instructions);
            });
        });

        try {
            await courseService.updateCourse(id, data);
            alert("Всичко е запазено!");
            navigate('/my-courses');
        } catch (err) { alert("Грешка!"); }
    };

    if (loading) return <div className="loader">Зареждане...</div>;

    return (
        <div className="edit-container">
            <form onSubmit={handleSubmit} className="course-edit-form">
                <header className="edit-header">
                    <h1>⚙️ Редакция на курс: {courseData.title}</h1>
                    <button type="submit" className="save-btn">Запази всички промени</button>
                </header>

                {/* --- СЕКЦИЯ: КУРС --- */}
                <section className="card main-info">
                    <h2>Обща информация</h2>
                    <div className="grid-input">
                        <div className="input-group">
                            <label>Заглавие на курса</label>
                            <input type="text" value={courseData.title} onChange={e => setCourseData({...courseData, title: e.target.value})} />
                        </div>
                        <div className="input-group">
                            <label>Цена (лв.)</label>
                            <input type="number" value={courseData.price} onChange={e => setCourseData({...courseData, price: e.target.value})} />
                        </div>
                    </div>
                    
                    <div className="video-toggle-section">
                        <label>Превю Видео:</label>
                        <div className="radio-group">
                            <label>
                                <input type="radio" checked={courseData.previewSource === 'link'} onChange={() => setCourseData({...courseData, previewSource: 'link'})} /> Линк
                            </label>
                            <label>
                                <input type="radio" checked={courseData.previewSource === 'file'} onChange={() => setCourseData({...courseData, previewSource: 'file'})} /> Файл
                            </label>
                        </div>
                        {courseData.previewSource === 'link' ? (
                            <input type="text" placeholder="YouTube URL..." value={courseData.previewVideoUrl} onChange={e => setCourseData({...courseData, previewVideoUrl: e.target.value})} />
                        ) : (
                            <input type="file" accept="video/*" onChange={e => setCourseData({...courseData, previewVideoFile: e.target.files[0]})} />
                        )}
                    </div>
                </section>

                {/* --- СЕКЦИЯ: УРОЦИ --- */}
                <section className="lessons-container">
                    <h2>📚 Списък с уроци</h2>
                    {courseData.lessons.map((lesson, lIdx) => (
                        <div key={lIdx} className="card lesson-card">
                            <div className="lesson-top">
                                <h3>Урок {lIdx + 1}</h3>
                                <button type="button" className="delete-lesson" onClick={() => {
                                    const newLessons = courseData.lessons.filter((_, i) => i !== lIdx);
                                    setCourseData({...courseData, lessons: newLessons});
                                }}>❌ Премахни</button>
                            </div>

                            <input className="lesson-title-input" type="text" value={lesson.lessonTitle} onChange={e => {
                                const copy = [...courseData.lessons];
                                copy[lIdx].lessonTitle = e.target.value;
                                setCourseData({...courseData, lessons: copy});
                            }} placeholder="Заглавие на урока" />

                            <textarea value={lesson.description} onChange={e => {
                                const copy = [...courseData.lessons];
                                copy[lIdx].description = e.target.value;
                                setCourseData({...courseData, lessons: copy});
                            }} placeholder="Кратко описание на урока..." />

                            {/* Видео източник за урока */}
                            <div className="video-toggle-section small">
                                <div className="radio-group">
                                    <label><input type="radio" checked={lesson.videoSource === 'link'} onChange={() => {
                                        const copy = [...courseData.lessons];
                                        copy[lIdx].videoSource = 'link';
                                        setCourseData({...courseData, lessons: copy});
                                    }} /> YouTube Линк</label>
                                    <label><input type="radio" checked={lesson.videoSource === 'file'} onChange={() => {
                                        const copy = [...courseData.lessons];
                                        copy[lIdx].videoSource = 'file';
                                        setCourseData({...courseData, lessons: copy});
                                    }} /> Качи Видео</label>
                                </div>
                                {lesson.videoSource === 'link' ? (
                                    <input type="text" value={lesson.videoUrl} onChange={e => {
                                        const copy = [...courseData.lessons];
                                        copy[lIdx].videoUrl = e.target.value;
                                        setCourseData({...courseData, lessons: copy});
                                    }} />
                                ) : (
                                    <input type="file" accept="video/*" onChange={e => {
                                        const copy = [...courseData.lessons];
                                        copy[lIdx].videoFile = e.target.files[0];
                                        setCourseData({...courseData, lessons: copy});
                                    }} />
                                )}
                            </div>

                            {/* ЗАДАНИЯ */}
                            <div className="assignments-area">
                                <h4>📝 Задания</h4>
                                {lesson.assignments.map((as, aIdx) => (
                                    <div key={aIdx} className="as-row">
                                        <input type="text" value={as.instructions} onChange={e => {
                                            const copy = [...courseData.lessons];
                                            copy[lIdx].assignments[aIdx].instructions = e.target.value;
                                            setCourseData({...courseData, lessons: copy});
                                        }} />
                                        <button type="button" onClick={() => {
                                            const copy = [...courseData.lessons];
                                            copy[lIdx].assignments.splice(aIdx, 1);
                                            setCourseData({...courseData, lessons: copy});
                                        }}>🗑️</button>
                                    </div>
                                ))}
                                <button type="button" className="add-as-btn" onClick={() => {
                                    const copy = [...courseData.lessons];
                                    copy[lIdx].assignments.push({ instructions: '' });
                                    setCourseData({...courseData, lessons: copy});
                                }}>+ Добави задание</button>
                            </div>
                        </div>
                    ))}
                    <button type="button" className="add-lesson-main" onClick={() => setCourseData({
                        ...courseData, 
                        lessons: [...courseData.lessons, { lessonTitle: '', description: '', videoUrl: '', assignments: [], videoSource: 'link' }]
                    })}>+ Добави нов урок към курса</button>
                </section>
            </form>
        </div>
    );
}