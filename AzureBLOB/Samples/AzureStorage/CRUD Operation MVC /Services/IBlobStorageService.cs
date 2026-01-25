
namespace AzureBLOBsample.Services
{
    public interface IBlobStorageService
    {
        Task<string> GetBlobUrl(string blobname);
        Task RemoveBlob(string blobname);
        Task<string> UploadBlob(IFormFile file, string imagename);
    }
}
