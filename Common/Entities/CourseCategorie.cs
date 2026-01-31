using System;

namespace CodingCourses.Common.Entities;

public class CourseCategory
{
    public int CourseId { get; set; }
    public Course? Course { get; set; }

    public int CategoryId { get; set; }
    public Category? Category { get; set; }

}
