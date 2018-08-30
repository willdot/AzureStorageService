using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BlobStorageService.Service
{
    public class AzureProvider : IAzureProvider
    {
        public CloudStorageAccount StorageAccount { get; set; }
        public CloudBlobClient Client { get; set; }

        private readonly AppSettings settings;

        public AzureProvider(IOptions<AppSettings> options)
        {
            settings = options.Value;

            CreateAccount();

            CreateClient();
        }

        public void CreateAccount()
        {
            CloudStorageAccount account;

            if (!CloudStorageAccount.TryParse(settings.ConnectionString, out account))
            {
                //throw new Exception("Error with account");
            }

            this.StorageAccount = account;
        }

        public void CreateClient()
        {
            this.Client = this.StorageAccount.CreateCloudBlobClient();
        }

        
    }
}