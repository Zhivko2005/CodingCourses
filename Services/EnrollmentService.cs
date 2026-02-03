using System;
using CodingCourses.Common.Entities;
using CodingCourses.Data;
using CodingCourses.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CodingCourses.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly AppDbContext _context;
    public EnrollmentService(AppDbContext context)
    {
        _context = context;
    }
    public EnrollmentResponseDto? EnrollStudent(EnrollmentRequestDto dto, int currentUserId, bool isAdmin)
    {
    
        var course = _context.Courses.Find(dto.CourseId);
        if (course == null)
        {
            return null;
        }

    
        int targetStudentId = dto.StudentId ?? currentUserId;

    
        bool canEnroll = false;
        if (isAdmin)
        {
             canEnroll = true;
        }
        else if (course.TeacherId == currentUserId) 
        {
            canEnroll = true; 
        }
        else if (targetStudentId == currentUserId) 
        {
            canEnroll = true;
        }
        if (!canEnroll) return null;

    
        var alreadyEnrolled = _context.Enrollments
            .Any(e => e.CourseId == dto.CourseId && e.UserId == targetStudentId);
    
        if (alreadyEnrolled) return null;

        var enrollment = new Enrollment
        {
            UserId = targetStudentId,
            CourseId = dto.CourseId,
            EnrollmentDate = DateTime.UtcNow
        };

        _context.Enrollments.Add(enrollment);
        _context.SaveChanges();

        return MapToDto(enrollment);
    }
    public IEnumerable<EnrollmentResponseDto> GetMyEnrollments(int userId)
    {
        var enrollments = _context.Enrollments
            .Include (e => e.Course)
            .Include (e => e.User)
            .Where(e => e.UserId == userId)
            .Select(e => MapToDto(e))
            .ToList();
        return enrollments;
    }
    public IEnumerable<EnrollmentResponseDto> GetCourseParticipants(int courseId, int instructorId, bool isAdmin)
    {
        var course = _context.Courses.Find(courseId);
        if (course == null) 
        {
            return new List<EnrollmentResponseDto>();
        }
        if (isAdmin || course.TeacherId == instructorId)
        {
            return _context.Enrollments
                .Include(e => e.User)
                .Include(e => e.Course)
                .Where(e => e.CourseId == courseId)
                .Select(e => MapToDto(e))
                .ToList();
        }

        return new List<EnrollmentResponseDto>();
    }
    public bool UnenrollStudent(int courseId, int targetStudentId, int currentUserId, bool isAdmin)
    { 
        var enrollment = _context.Enrollments
            .Include(e => e.Course)
            .FirstOrDefault(e => e.CourseId == courseId && e.UserId == targetStudentId);

        if (enrollment == null)
        {
            return false;
        }
 
        bool canDelete = false;

        if (isAdmin)
        { 
            canDelete = true;
        }
        else if (currentUserId == targetStudentId)
        { 
            canDelete = true;
        }
        else if (enrollment.Course.TeacherId == currentUserId)
        { 
            canDelete = true;
        }

        if (canDelete)
        {
            _context.Enrollments.Remove(enrollment);
            _context.SaveChanges();
            return true;
        }

        return false;
    }
    private static EnrollmentResponseDto MapToDto(Enrollment e) => new EnrollmentResponseDto
    {
        UserId = e.UserId,
        Username = e.User?.Username ?? "N/A",
        CourseId = e.CourseId,
        Title = e.Course?.Title ?? "N/A",
        EnrollmentDate = e.EnrollmentDate
    };
}
