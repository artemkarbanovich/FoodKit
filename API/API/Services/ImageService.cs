using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace API.Services;

public class ImageService : IImageService
{
    private readonly Cloudinary _cloudinary;

    public ImageService(IOptions<CloudinarySettings> config)
    {
        _cloudinary = new Cloudinary(new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret));
    }


    public async Task<ImageUploadResult> AddImageAsync(IFormFile imageFile)
    {
        var uploadResult = new ImageUploadResult();

        if(imageFile.Length > 0)
        {
            using var stream = imageFile.OpenReadStream();
            var uploadParams = new ImageUploadParams { File = new FileDescription(imageFile.FileName, stream) };

            uploadResult = await _cloudinary.UploadAsync(uploadParams);
        }

        return uploadResult;
    }

    public async Task<DeletionResult> DeleteImageAsync(string publicId)
    {
        return await _cloudinary.DestroyAsync(new DeletionParams(publicId));
    }
}