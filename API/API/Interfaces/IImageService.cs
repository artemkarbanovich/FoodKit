using CloudinaryDotNet.Actions;

namespace API.Interfaces;

public interface IImageService
{
    Task<ImageUploadResult> AddImageAsync(IFormFile imageFile);
    Task<DeletionResult> DeleteImageAsync(string publicId);
}