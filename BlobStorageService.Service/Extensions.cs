using System;
using System.Net;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BlobStorageService.Service
{
    public static class Extensions
    {
        public static Exception Convert(this Exception e)
        {
            // Try and convert the exception to an Azure Storage Exception
            var storageException = (e as StorageException) ?? e.InnerException as StorageException;

            // If the storage exception if not null because it's an Azure Storage Exception, then convert it
            if (storageException != null)
            {
                StorageErrorCode errorCode;

                switch ((HttpStatusCode)storageException.RequestInformation.HttpStatusCode)
                {
                    case HttpStatusCode.Forbidden:
                        errorCode = StorageErrorCode.InvalidCredentials;
                        break;
                    case HttpStatusCode.NotFound:
                        errorCode = StorageErrorCode.InvalidName;
                        break;
                    default:
                        errorCode = StorageErrorCode.GenericException;
                        break;
                }

                return new StorageException(errorCode.ToString());
            }

            // If exception isn't an Azure Storage Exception, then return the incoming exception
            return e;
        }

        public static bool IsAzureStorageException(this Exception e)
        {
            return e is StorageException || e.InnerException is StorageException;
        }
    }
}