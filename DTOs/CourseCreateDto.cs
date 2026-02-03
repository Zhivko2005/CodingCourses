using System;

namespace CodingCourses.DTOs;

public class CourseCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public List<int> CategoryIds { get; set; } = new List<int>();
}
