using System;
using CodingCourses.DTOs;

namespace CodingCourses.Services;

public interface IEnrollmentService
{
    EnrollmentResponseDto? EnrollStudent(EnrollmentRequestDto dto, int currentUserId, bool isAdmin);
    IEnumerable<EnrollmentResponseDto> GetMyEnrollments(int studentId);
    IEnumerable<EnrollmentResponseDto> GetCourseParticipants(int courseId, int instructorId, bool isAdmin);
    bool UnenrollStudent(int courseId, int targetStudentId, int currentUserId, bool isAdmin);

}
