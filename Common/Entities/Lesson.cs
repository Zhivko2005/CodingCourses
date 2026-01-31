using System;

namespace CodingCourses.Common.Entities;

public class Lesson
{
    public int Id { get; set; }
    public string LessonTitle { get; set; } = null!;
    public string Content { get; set; } = null!;    
    public int CourseId { get; set; } 
    public Course Course { get; set; } = null!;
}
