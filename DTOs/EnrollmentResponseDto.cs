using System;

namespace CodingCourses.DTOs;

public class EnrollmentResponseDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty; 
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime EnrollmentDate { get; set; }
}
