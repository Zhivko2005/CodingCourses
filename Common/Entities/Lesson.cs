using System;

namespace CodingCourses.Common.Entities;

public class Lesson
{
    public int Id { get; set; }
    public string LessonTitle { get; set; } = null!;
    public string Description { get; set; } = null!;    
    public string VideoUrl { get; set; } = null!;
    public int CourseId { get; set; } 
    public Course Course { get; set; } = null!;
    public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
}
 