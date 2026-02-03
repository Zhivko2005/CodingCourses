using System;
using CodingCourses.DTOs;

namespace CodingCourses.Services;

public interface ILessonService
{
    IEnumerable<LessonResponseDto> GetLessonsByCourse(int courseId);
    LessonResponseDto? GetLessonById(int id);
    LessonResponseDto? CreateLesson(LessonCreateDto dto, int currentUserId);
    LessonResponseDto? UpdateLesson(int id, LessonCreateDto dto, int currentUserId);
    bool DeleteLesson(int id, int currentUserId, bool isAdmin);

}
