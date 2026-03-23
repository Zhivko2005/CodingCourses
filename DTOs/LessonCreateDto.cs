using System;

namespace CodingCourses.DTOs;

public class LessonCreateDto
{
    public string Title { get; set; } = null!; 
    public string Description { get; set; } = null!;
    public string VideoUrl { get; set; } = null!;
    public int CourseId { get; set; }
}
 