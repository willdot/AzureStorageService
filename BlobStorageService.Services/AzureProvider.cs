using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BlobStorageService.Service
{
    /// <summary>
    /// This class deals with the connection and configuration of an Azure Blob storage account
    /// </summary>
    public class AzureProvider : IAzureProvider
    {
        public CloudStorageAccount StorageAccount { get; private set; }
        public CloudBlobClient Client { get; private set; }

        private readonly AppSettings settings;

        public AzureProvider(IOptions<AppSettings> options)
        {
            settings = options.Value;
        }

        /// <summary>
        /// Sets up the connection to the Azure blob storage account and creates a client to use.
        /// </summary>
        public void Initialize()
        {
            this.StorageAccount = CreateAccount();

            this.Client = CreateClient();
        }

        /// <summary>
        /// Creates a CloudStorageAccount from the connection string provided.
        /// </summary>
        protected CloudStorageAccount CreateAccount()
        {
            if (settings == null)
            {
                throw new NullReferenceException("No App settings");
            }

            CloudStorageAccount account;
            
            if (!CloudStorageAccount.TryParse(settings.ConnectionString, out account))
            {
                throw new Exception("Error with account");
            }

            return account;
        }

        /// <summary>
        /// Creates a client that can be used to connect to the Blob Storage.
        /// </summary>
        protected CloudBlobClient CreateClient()
        {
            return this.StorageAccount.CreateCloudBlobClient();
        }
    }
}