using System;

namespace BlobStorageService.Service
{
    public interface IBlobStorageTools
    {
        void Upload(string containerReference, string filename);

        void Download(string containerReference, string remoteFilename, string localFileLocation);

        void Delete(string containerReference, string filename);

        void Move (string sourceContainerReference, string sourceFilename,  string destinationContainerReference, string destinationFilename);

        void Copy (string sourceContainerReference, string sourceFilename,  string destinationContainerReference, string destinationFilename);

        void DeleteContainer(string containerReference);

        void CreateContainer(string containerReference);

    }
}