using System;

namespace CodingCourses.DTOs;

public class AssignmentResponseDto
{
    public int Id { get; set; }
    public string Instructions { get; set; } = string.Empty;
    public int CourseId { get; set; }
    public int LessonId { get; set; }
}
 