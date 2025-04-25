using CloudinaryDotNet.Actions;

namespace DatingAppAPI.Interfaces
{
    public interface IPhotoInterface
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
