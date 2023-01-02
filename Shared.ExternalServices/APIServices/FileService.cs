using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Shared.ExternalServices.Configurations;
using Shared.ExternalServices.Constant;
using Shared.ExternalServices.DTOs;
using Shared.ExternalServices.Enums;
using Shared.ExternalServices.Interfaces;

namespace Shared.ExternalServices.APIServices
{
    public class FileService : IFileService
    {

        public async Task<UploadResponse> FileUpload(IFormFile file, UploadFolderEnum featureFolder, CancellationToken cancellationToken)
        {
            string folderName = FileFolderHandler.GetFolderName(featureFolder);
            
            BlobServiceClient blobServiceClient = new(Settings.FILE_UPLOAD_PATH);
            BlobContainerClient blobContainerClient = await blobServiceClient.CreateBlobContainerAsync(folderName, PublicAccessType.BlobContainer, null, cancellationToken);
            BlobClient blobClient = blobContainerClient.GetBlobClient(file.FileName);

            var memory = new MemoryStream();
            await file.CopyToAsync(memory, cancellationToken);
            memory.Position = 0;

            await blobClient.UploadAsync(memory, true, cancellationToken);
            UploadResponse fileUrl = new()
            {
                CloudUrl = blobClient.Uri.AbsolutePath,
                PublicUrl = blobClient.Uri.AbsoluteUri
            };
            memory.Close();
            return fileUrl;
        }
        
        public async Task<string> ContainerDeletion(string cloudpath, CancellationToken cancellationToken)
        {
            var tokens = cloudpath.Split('/');
            string folderName = tokens[1];

            BlobServiceClient blobServiceClient = new(Settings.FILE_UPLOAD_PATH);
            var blobContainerClient = await blobServiceClient.DeleteBlobContainerAsync(folderName, null, cancellationToken);
         
            return blobContainerClient.ToString();
        }
    }
}
