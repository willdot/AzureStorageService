using System;
using System.IO;
using System.Threading.Tasks;
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
        public async Task DownloadAsync(string containerReference, string remoteFilename, string localFileLocation)
        {
            CloudBlobContainer container = GetContainer(containerReference);

            CloudBlockBlob blockBlob = GetBlockBlob(container, remoteFilename);

            await blockBlob.DownloadToFileAsync(localFileLocation, FileMode.Create);

            using (var fileStream = File.OpenWrite(localFileLocation))
            {
                fileStream.Position = 0;
                await blockBlob.DownloadToStreamAsync(fileStream);
            }
        }

        /// <summary>
        /// Upload a file to blob storage
        /// </summary>
        /// <param name="containerReference">Reference of container to upload file to</param>
        /// <param name="filename">Full path of file to upload</param>
        public async Task UploadAsync(string containerReference, string filename)
        {
            CloudBlobContainer container = GetContainer(containerReference);

            CloudBlockBlob blockBlob = GetBlockBlob(container, filename);

            try
            {
                await blockBlob.UploadFromFileAsync(filename);
            }
            catch (Exception ex)
            {
                throw ex.ParseToStorageException();
            }
        }

        /// <summary>
        /// Delete a file
        /// </summary>
        /// <param name="containerReference">Container reference that contains the file to delete</param>
        /// <param name="filename">Filename of the file to delete</param>
        public async Task DeleteAsync(string containerReference, string filename)
        {
            try
            {
                await GetContainer(containerReference).DeleteAsync();
            }
            catch (Exception ex)
            {
                throw ex.ParseToStorageException();
            }
        }

        /// <summary>
        /// Move a file from one container to another
        /// </summary>
        /// <param name="sourceContainerReference">Container reference of the source file</param>
        /// <param name="sourceFilename">Filename of the source file</param>
        /// <param name="destinationContainerReference">Container reference of the destination</param>
        /// <param name="destinationFilename">Filename of the destination file</param>
        public async Task MoveAsync(string sourceContainerReference, string sourceFilename, string destinationContainerReference, string destinationFilename)
        {
            try
            {
                await CopyAsync(sourceContainerReference, sourceFilename, destinationContainerReference, destinationFilename);
                await DeleteAsync(sourceContainerReference, sourceFilename);
            }
            catch (Exception ex)
            {
                throw ex.ParseToStorageException();
            }
        }

        /// <summary>
        /// Copy a fle from one container to another
        /// </summary>
        /// <param name="sourceContainerReference">Container reference of the source file</param>
        /// <param name="sourceFilename">Filename of the source file</param>
        /// <param name="destinationContainerReference">Container reference of the destination</param>
        /// <param name="destinationFilename">Filename of the destination file</param>
        public async Task CopyAsync(string sourceContainerReference, string sourceFilename, string destinationContainerReference, string destinationFilename)
        {
            var sourceContainer = GetContainer(sourceContainerReference);
            var sourceBlob = GetBlockBlob(sourceContainer, sourceFilename);

            var destinationContainer = GetContainer(destinationContainerReference);

            await destinationContainer.CreateIfNotExistsAsync();

            var destinationBlob = destinationContainer.GetBlockBlobReference(destinationFilename);

            try
            {
                await destinationBlob.StartCopyAsync(sourceBlob);

                if (destinationBlob.CopyState.Status != CopyStatus.Success)
                {
                    throw new Exception($"There was a problem copying the file: {destinationBlob.CopyState.Status}");
                }
            }
            catch (Exception ex)
            {
                throw ex.ParseToStorageException();
            }
        }

        /// <summary>
        /// Create a new container
        /// </summary>
        /// <param name="containerReference">Container reference of container to create</param>
        public async Task CreateContainerAsync(string containerReference)
        {
            var destinationContainer = GetContainer(containerReference);

            await destinationContainer.CreateIfNotExistsAsync();
        }

        /// <summary>
        /// Delete a container
        /// </summary>
        /// <param name="containerReference">Container reference of container to delete</param>
        public async Task DeleteContainerAsync(string containerReference)
        {
            var sourceContainer = GetContainer(containerReference);
            await sourceContainer.DeleteAsync();
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