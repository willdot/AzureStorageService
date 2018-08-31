using System;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BlobStorageService.Service
{
    /// <summary>
    /// This class allows the manipulation of files with Azure Blob storage
    /// </summary>
    public class BlobStorageTools : IBlobStorageTools
    {

        private IAzureProvider Provider;

        public BlobStorageTools(IAzureProvider provider)
        {
            Provider = provider;
        }

        /// <summary>
        /// Download a file from blob storage
        /// </summary>
        /// <param name="containerReference">Container reference to download file from</param>
        /// <param name="remoteFilename">File name to download.null Case sensitive</param>
        /// <param name="localFileLocation">Full path including filename of where to download to</param>
        public void Download(string containerReference, string remoteFilename, string localFileLocation)
        {
            CloudBlobContainer container = GetContainer(containerReference);

            CloudBlockBlob blockBlob = GetBlockBlob(container, remoteFilename);

            //string fullPath = Path.Combine(fileLocation, localFileName);

            blockBlob.DownloadToFileAsync(localFileLocation, FileMode.Create).Wait();

            using (var fileStream = File.OpenWrite(localFileLocation))
            {
                fileStream.Position = 0;
                blockBlob.DownloadToStreamAsync(fileStream);    
            }
        }

        /// <summary>
        /// Upload a file to blob storage
        /// </summary>
        /// <param name="containerReference">Reference of container to upload file to</param>
        /// <param name="filename">Full path of file to upload</param>
        public void Upload(string containerReference, string filename)
        {
            CloudBlobContainer container = GetContainer(containerReference);

            CloudBlockBlob blockBlob = GetBlockBlob(container, filename);

            blockBlob.UploadFromFileAsync(filename).Wait();
        }


        public void Delete(string containerReference, string filename)
        {
            throw new NotImplementedException();
        }

        public void Move(string sourceContainerReference, string sourceFilename, string destinationContainerReference, string destinationFilename)
        {
            throw new NotImplementedException();
        }

        public void Copy(string sourceContainerReference, string sourceFilename, string destinationContainerReference, string destinationFilename)
        {
            throw new NotImplementedException();
        }

        
        private CloudBlobContainer GetContainer(string containerReference)
        {
            return Provider.Client.GetContainerReference(containerReference); 
        }

        private CloudBlockBlob GetBlockBlob(CloudBlobContainer container, string blobName)
        {
            return container.GetBlockBlobReference(blobName);
        }

        
    }
}