using System;

namespace CodingCourses.Common.Entities;

public class Assignment
{
    public int Id { get; set; } 
    public string Instructions  { get; set; } = null!;
    public int CourseId { get; set; } 
    public Course Course { get; set; } = null!;

    public ICollection<Submission> Submissions { get; set; } = new List<Submission>(); 

}
