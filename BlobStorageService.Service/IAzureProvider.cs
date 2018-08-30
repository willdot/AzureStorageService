using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BlobStorageService.Service
{
    public interface IAzureProvider
    {
        CloudStorageAccount StorageAccount {get; set;}

        CloudBlobClient Client {get; set;}


        void CreateAccount();

        void CreateClient();
    }
}
