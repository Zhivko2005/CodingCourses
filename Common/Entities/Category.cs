using System;

namespace CodingCourses.Common.Entities;

public class Category
{
    public int Id { get; set; }
    public string CategoryName { get; set; } = null!;

    public ICollection<CourseCategory> CourseCategories { get; set; } = new List<CourseCategory>();
}
