using System;
using CodingCourses.DTOs;

namespace CodingCourses.Services;

public interface ISubmissionService
{
    SubmissionResponseDto? CreateSubmission(SubmissionCreateDto dto, int studentId);
    IEnumerable<SubmissionResponseDto> GetSubmissions(int assignmentId, int currentUserId, bool isAdmin, bool isInstructor);
}
