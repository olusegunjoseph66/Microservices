using Microsoft.AspNetCore.Http;
using Shared.ExternalServices.DTOs;
using Shared.ExternalServices.Enums;

namespace Shared.ExternalServices.Interfaces
{
    public interface IFileService
    {
        Task<UploadResponse> FileUpload(IFormFile file, UploadFolderEnum featureFolder, CancellationToken cancellationToken);

        Task<string> ContainerDeletion(string cloudpath, CancellationToken cancellationToken);
    }
}
