using System;

namespace CodingCourses.DTOs;

public class LessonResponseDto
{
    public int Id { get; set; }
    public string LessonTitle { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int CourseId { get; set; }
}
