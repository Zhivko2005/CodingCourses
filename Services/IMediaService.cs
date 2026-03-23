using System;
using CodingCourses.DTOs;

namespace CodingCourses.Services;

public interface IMediaService
{
    Task<VideoUploadDto> UploadVideoAsync(IFormFile file);
}
