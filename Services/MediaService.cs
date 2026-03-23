using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using CodingCourses.Helpers;
using CodingCourses.DTOs;
using CodingCourses.Services;  

public class MediaService : IMediaService
{
    private readonly Cloudinary _cloudinary;

    public MediaService(IOptions<CloudinarySettings> config)
    {
        var acc = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );
        _cloudinary = new Cloudinary(acc);
    }

  public async Task<VideoUploadDto> UploadVideoAsync(IFormFile file)
{
    var uploadResult = new VideoUploadDto();
    if (file == null || file.Length == 0) return uploadResult;

    using var stream = file.OpenReadStream();
    var uploadParams = new VideoUploadParams
    {
        File = new FileDescription(file.FileName, stream),
        Folder = "course-lessons"
    };

    var result = await _cloudinary.UploadAsync(uploadParams);

    if (result.Error != null) throw new Exception(result.Error.Message);
 
    uploadResult.Url = result.SecureUrl.AbsoluteUri; 
    uploadResult.PublicId = result.PublicId;

    return uploadResult;
}
}