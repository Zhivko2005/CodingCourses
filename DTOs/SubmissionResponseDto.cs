using System;

namespace CodingCourses.DTOs;

public class SubmissionResponseDto
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public int AssignmentId { get; set; }
    public int UserId { get; set; }
}
