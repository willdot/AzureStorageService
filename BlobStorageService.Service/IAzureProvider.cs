using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BlobStorageService.Service
{
    public interface IAzureProvider
    {
        CloudStorageAccount StorageAccount {get;}

        CloudBlobClient Client {get;}


        void CreateAccount();

        void CreateClient();
    }
}
