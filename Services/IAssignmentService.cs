using System;
using CodingCourses.DTOs;

namespace CodingCourses.Services;

public interface IAssignmentService
{
    IEnumerable<AssignmentResponseDto> GetByCourse(int courseId);
    AssignmentResponseDto? CreateAssignment(AssignmentCreateDto dto, int currentUserId, bool isAdmin);
    AssignmentResponseDto? UpdateAssignment(int id, AssignmentCreateDto dto, int currentUserId);
    bool DeleteAssignment(int id, int currentUserId, bool isAdmin);
}
