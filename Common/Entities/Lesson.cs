using System;

namespace CodingCourses.Common.Entities;

public class Lesson
{
    public int Id { get; set; }
    public string LessonTitle { get; set; } = null!;
    public string Content { get; set; } = null!;    
    public LessonType Type { get; set; }
    public int CourseId { get; set; } 
    public Course Course { get; set; } = null!;
    public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
}
public enum LessonType
{
    Text = 1,
    Video = 2,
}
