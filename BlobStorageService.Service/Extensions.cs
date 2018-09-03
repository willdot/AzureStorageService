using System;
using System.Net;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BlobStorageService.Service
{
    public static class Extensions
    {
        public static Exception ParseToStorageException(this Exception e)
        {
            // Try and convert the inner exception to an Azure Storage Exception
            StorageException storageException = e.InnerException as StorageException;

            // Handle the storage exception to extract the Request information
            if (storageException != null)
            {
                return new StorageException(storageException.RequestInformation.HttpStatusMessage, storageException);
            } 

            // Not an azure exception, so return original exception 
            return e;
        }
    }
}