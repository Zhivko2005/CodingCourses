using System;
using CodingCourses.Common.Entities;
using CodingCourses.Data;
using CodingCourses.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CodingCourses.Services;

public class SubmissionService : ISubmissionService
{
    private readonly AppDbContext _context;
    public SubmissionService(AppDbContext context)
    {
        _context = context;  
    } 

    public SubmissionResponseDto? CreateSubmission(SubmissionCreateDto dto, int studentId)
    {
        var assignment = _context.Assignments.Find(dto.AssignmentId);
        if (assignment == null) 
        {
            return null;
        }

        var isEnrolled = _context.Enrollments.Any(e => e.CourseId == assignment.CourseId && e.UserId == studentId);
        if (isEnrolled == false) 
        {
            return null;
        }
        var submission = new Submission { Content = dto.Content, AssignmentId = dto.AssignmentId, UserId = studentId };
        _context.Submissions.Add(submission);
        _context.SaveChanges();

        return new SubmissionResponseDto 
        {
            Id = submission.Id, Content = submission.Content,
            AssignmentId = submission.AssignmentId,
            UserId = studentId 
        };
    }

    public IEnumerable<SubmissionResponseDto> GetSubmissions(int assignmentId, int currentUserId, bool isAdmin, bool isInstructor)
    {
        var assignment = _context.Assignments
            .Include(a => a.Course)
            .FirstOrDefault(a => a.Id == assignmentId);

        if (assignment == null)
        {
            return new List<SubmissionResponseDto>();
        }

    
        if (isAdmin || (isInstructor && assignment.Course.TeacherId == currentUserId))
        {
            return _context.Submissions
                .Where(s => s.AssignmentId == assignmentId)
                .Select(s => new SubmissionResponseDto 
                { 
                    Id = s.Id, 
                    Content = s.Content, 
                    AssignmentId = s.AssignmentId, 
                    UserId = s.UserId 
                })
                .ToList();
        }

        return _context.Submissions
            .Where(s => s.AssignmentId == assignmentId && s.UserId == currentUserId)
            .Select(s => new SubmissionResponseDto 
            { 
                Id = s.Id, 
                Content = s.Content, 
                AssignmentId = s.AssignmentId, 
                UserId = s.UserId 
            })
            .ToList();
        }
}
