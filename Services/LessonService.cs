using CodingCourses.Common.Entities;
using CodingCourses.Data;
using CodingCourses.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CodingCourses.Services;

public class LessonService : ILessonService
{
    private readonly AppDbContext _context;
    public LessonService(AppDbContext context)
    {
      _context = context;  
    } 

    public IEnumerable<LessonResponseDto> GetLessonsByCourse(int courseId)
    {
        var lessons = _context.Lessons
            .Where(l => l.CourseId == courseId)
            .ToList();

        var dtos = new List<LessonResponseDto>();
        foreach (var l in lessons)
        {
            dtos.Add(new LessonResponseDto {
                Id = l.Id,
                LessonTitle = l.LessonTitle,
                Content = l.Content,
                CourseId = l.CourseId
            });
        }
        return dtos;
    }

    public LessonResponseDto? GetLessonById(int id)
    {
        var l = _context.Lessons.Find(id);
        
        if (l == null)
        {
            return null;
        }

        return new LessonResponseDto { 
            Id = l.Id, 
            LessonTitle = l.LessonTitle, 
            Content = l.Content, 
            CourseId = l.CourseId 
        };
    }

    public LessonResponseDto? CreateLesson(LessonCreateDto dto, int currentUserId)
    {
        var course = _context.Courses.Find(dto.CourseId);
        
        if (course == null) 
        {
            return null;
        }
        if (course.TeacherId != currentUserId)
        {
             return null;
        }

        var lesson = new Lesson { 
            LessonTitle = dto.Title, 
            Content = dto.Content, 
            CourseId = dto.CourseId 
        };

        _context.Lessons.Add(lesson);
        _context.SaveChanges();
        
        return GetLessonById(lesson.Id);
    }

    public LessonResponseDto? UpdateLesson(int id, LessonCreateDto dto, int currentUserId)
    {
        var lesson = _context.Lessons.Include(l => l.Course).FirstOrDefault(l => l.Id == id);
        
        if (lesson == null) 
        {
            return null;
        }
        if (lesson.Course.TeacherId != currentUserId) 
        {
            return null;
        }

        lesson.LessonTitle = dto.Title;
        lesson.Content = dto.Content;
        
        _context.SaveChanges();
        return GetLessonById(lesson.Id);
    }

    public bool DeleteLesson(int id, int currentUserId, bool isAdmin)
    {
        var lesson = _context.Lessons.Include(l => l.Course).FirstOrDefault(l => l.Id == id);
        
        if (lesson == null) 
        {
            return false;
        }
        if (isAdmin || lesson.Course.TeacherId == currentUserId)
        {
            _context.Lessons.Remove(lesson);
            _context.SaveChanges();
            return true;
        }

        return false;
    }
}