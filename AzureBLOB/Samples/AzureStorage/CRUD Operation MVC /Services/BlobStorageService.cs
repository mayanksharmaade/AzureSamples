using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;

namespace AzureBLOBsample.Services
{
    public class BlobStorageService : IBlobStorageService
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
                BlobContainerClient container = new BlobContainerClient(_configuration["StorageConnectionStrings"], containername);
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


        public async Task<string> GetBlobUrl(string blobname)
        {
            var container = await GetBlobContainerClient();
            var blobclient = container.GetBlobClient(blobname);

            BlobSasBuilder sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = container.Name,
                BlobName = blobclient.Name,
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1),
                Protocol = SasProtocol.Https,
                Resource = "b"
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);
            return blobclient.GenerateSasUri(sasBuilder).ToString();

        }

        public async Task RemoveBlob(string blobname)
        {
            var container = await GetBlobContainerClient();
            var blobclient = container.GetBlobClient(blobname);
            await blobclient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);

        }

    }
}
