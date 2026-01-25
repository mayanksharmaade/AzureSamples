using Azure.Storage.Blobs;

namespace AzureBLOBsample.Services
{
    public class BlobStorageService
    {
        private readonly IConfiguration _configuration;
        private string containername = "userimages";
        public BlobStorageService(IConfiguration config)
        {
            _configuration = config;
        }

        private async Task<BlobContainerClient> GetBlobContainerClient()
        {
            try
            {
                BlobContainerClient container = new BlobContainerClient(_configuration["StorageConnectionStrings"],containername);
                await container.CreateIfNotExistsAsync();
                return container;
            }
             catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<string> UploadBlob(IFormFile file, string imagename)
        {
            var blobname = $"{imagename}{Path.GetExtension(file.FileName)}";
            var containername = await GetBlobContainerClient();
            using var memorystream = new MemoryStream();
             file.CopyTo(memorystream);
            memorystream.Position = 0;
            var blobclient = await containername.UploadBlobAsync(blobname, memorystream, default);
            return blobname;
        }
    }
}
