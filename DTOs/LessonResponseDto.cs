using System;

namespace CodingCourses.DTOs;

public class LessonResponseDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? VideoUrl { get; set; }
    public int CourseId { get; set; }
    public List<AssignmentResponseDto> Assignments { get; set; } = new();
    
} 
