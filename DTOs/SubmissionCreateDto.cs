using System;

namespace CodingCourses.DTOs;

public class SubmissionCreateDto
{
    public string Content { get; set; } = string.Empty;
    public int AssignmentId { get; set; }
}