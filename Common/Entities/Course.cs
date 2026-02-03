using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodingCourses.Common.Entities;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; } 
    public int TeacherId { get; set; }  
    public User Teacher { get; set; } = null!;

    public ICollection<CourseCategory> CourseCategories { get; set; } = new List<CourseCategory>();
    public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>(); 

}
