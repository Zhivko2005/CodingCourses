using System;

namespace CodingCourses.Common.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Email { get; set; } = null!;

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
    public ICollection<Course> TaughtCourses { get; set; } = new List<Course>();
 
}
