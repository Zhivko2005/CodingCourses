using System;
using CodingCourses.Common.Entities;
using CodingCourses.Data;
using CodingCourses.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CodingCourses.Services;

public class AssignmentService : IAssignmentService
{
    private readonly AppDbContext _context;
    public AssignmentService(AppDbContext context) 
    {
        _context = context;
    }

    public IEnumerable<AssignmentResponseDto> GetByCourse(int courseId)
    {
        var assignments = _context.Assignments
            .Where(a => a.CourseId == courseId)
            .Select(a => new AssignmentResponseDto
            {
                Id = a.Id,
                Instructions = a.Instructions,
                CourseId = a.CourseId
            })
            .ToList();
        return assignments;
    }

    public AssignmentResponseDto? CreateAssignment(AssignmentCreateDto dto, int currentUserId, bool isAdmin)
    {
        var course = _context.Courses.Find(dto.CourseId);
        if (course == null)
        { 
            return null;
        }
        if (isAdmin == false && course.TeacherId != currentUserId) 
        {
            return null;
        }
        var assignment = new Assignment { Instructions = dto.Instructions, CourseId = dto.CourseId };
        _context.Assignments.Add(assignment);
        _context.SaveChanges();

        return new AssignmentResponseDto 
        {
            Id = assignment.Id, Instructions = assignment.Instructions, 
            CourseId = assignment.CourseId 
        };
    }

    public AssignmentResponseDto? UpdateAssignment(int id, AssignmentCreateDto dto, int currentUserId)
    {
        var assignment = _context.Assignments
        .Include(a => a.Course)
        .FirstOrDefault(a => a.Id == id);
        if (assignment == null)
        {
            return null;
        }
        if (assignment.Course.TeacherId != currentUserId)
        {
            return null;
        }
        assignment.Instructions = dto.Instructions;
        _context.SaveChanges();

        return new AssignmentResponseDto 
        { 
            Id = assignment.Id, Instructions = assignment.Instructions,
            CourseId = assignment.CourseId 
        };
    }

    public bool DeleteAssignment(int id, int currentUserId, bool isAdmin)
    {
        var assignment = _context.Assignments
        .Include(a => a.Course).
        FirstOrDefault(a => a.Id == id);
        if (assignment == null)
        {
            return false;  
        } 

        if (isAdmin || assignment.Course.TeacherId == currentUserId)
        {
            _context.Assignments.Remove(assignment);
            _context.SaveChanges();
            return true;
        }
        return false;
    }
}