using System;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BlobStorageService.Service
{
    public class BlobStorageTools : IBlobStorageTools
    {

        private IAzureProvider Provider;

        public BlobStorageTools(IAzureProvider provider)
        {
            Provider = provider;
        }

        public void Download(string containerReference, string fileLocation, string filename)
        {
            CloudBlobContainer container = GetContainer(containerReference);

            CloudBlockBlob blockBlob = GetBlockBlob(container, filename);

            string fullPath = Path.Combine(fileLocation, filename);

            blockBlob.DownloadToFileAsync(fullPath, FileMode.Create).Wait();

            using (var fileStream = File.OpenWrite(fullPath))
            {
                fileStream.Position = 0;
                blockBlob.DownloadToStreamAsync(fileStream);    
            }
        }

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