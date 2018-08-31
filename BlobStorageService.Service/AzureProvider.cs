using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BlobStorageService.Service
{
    public class AzureProvider : IAzureProvider
    {
        public CloudStorageAccount StorageAccount { get; private set; }
        public CloudBlobClient Client { get; private set; }

        private readonly AppSettings settings;

        public AzureProvider(IOptions<AppSettings> options)
        {
            settings = options.Value;
        }

        public void Initialize()
        {
            CreateAccount();

            CreateClient();
        }

        internal void CreateAccount()
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

            this.StorageAccount = account;
        }

        internal void CreateClient()
        {
            this.Client = this.StorageAccount.CreateCloudBlobClient();
        } 
    }
}