using System;
using System.Threading.Tasks;

namespace BlobStorageService.Service
{
    public interface IBlobStorageTools
    {
        Task UploadAsync(string containerReference, string filename);

        Task DownloadAsync(string containerReference, string remoteFilename, string localFileLocation);

        Task DeleteAsync(string containerReference, string filename);

        Task MoveAsync(string sourceContainerReference, string sourceFilename,  string destinationContainerReference, string destinationFilename);

        Task CopyAsync(string sourceContainerReference, string sourceFilename,  string destinationContainerReference, string destinationFilename);

        Task DeleteContainerAsync(string containerReference);

        Task CreateContainerAsync(string containerReference);

    }
}