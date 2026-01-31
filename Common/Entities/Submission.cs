using System;

namespace CodingCourses.Common.Entities;

public class Submission
{
    public int Id { get; set; }
    public string Content { get; set; } = null!; 
    public int AssignmentId { get; set; }
    public Assignment Assignment { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
