using System;

namespace CodingCourses.DTOs;

public class EnrollmentRequestDto
{
    public int CourseId { get; set; }
    public int? StudentId { get; set; }
}
