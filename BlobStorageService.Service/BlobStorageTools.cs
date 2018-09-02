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


        /// <summary>
        /// Delete a file
        /// </summary>
        /// <param name="containerReference">Reference of the container that the file is located in</param>
        /// <param name="filename">Filename of file to delete</param>
        public void Delete(string containerReference, string filename)
        {
            GetContainer(containerReference).DeleteIfExistsAsync().Wait();
        }

        /// <summary>
        /// Move a file from one container to another
        /// </summary>
        /// <param name="sourceContainerReference">Container reference of the source file</param>
        /// <param name="sourceFilename">Filename of the source file</param>
        /// <param name="destinationContainerReference">Container reference of the destination</param>
        /// <param name="destinationFilename">Filename of the destination file</param>
        public void Move(string sourceContainerReference, string sourceFilename, string destinationContainerReference, string destinationFilename)
        {
            Copy(sourceContainerReference, sourceFilename, destinationContainerReference, destinationFilename);
            Delete(sourceContainerReference, sourceFilename);
        }

        /// <summary>
        /// Copy a fle from one container to another
        /// </summary>
        /// <param name="sourceContainerReference">Container reference of the source file</param>
        /// <param name="sourceFilename">Filename of the source file</param>
        /// <param name="destinationContainerReference">Container reference of the destination</param>
        /// <param name="destinationFilename">Filename of the destination file</param>
        public void Copy(string sourceContainerReference, string sourceFilename, string destinationContainerReference, string destinationFilename)
        {
            var sourceContainer = GetContainer(sourceContainerReference);
            var sourceBlob = GetBlockBlob(sourceContainer, sourceFilename);

            var destinationContainer = GetContainer(destinationContainerReference);

            destinationContainer.CreateIfNotExistsAsync().Wait();

            var destinationBlob = destinationContainer.GetBlockBlobReference(destinationFilename);

            destinationBlob.StartCopyAsync(sourceBlob).Wait();

            if (destinationBlob.CopyState.Status != CopyStatus.Success)
            {
                throw new Exception($"There was a problem copying the file: {destinationBlob.CopyState.Status}");
            }
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