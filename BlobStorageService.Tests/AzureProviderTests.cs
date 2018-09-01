using System;
using NUnit;
using NUnit.Framework;
using Moq;

using BlobStorageService.Service;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Auth;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BlobStorageService.Tests
{
    [TestFixture]
    public class AzureProviderTests
    {
        Mock<IOptions<AppSettings>> appSettingsMoq;

        Mock<IAzureProvider> azureProviderMoq;

        Mock<CloudStorageAccount> storageMoq;
        Mock<CloudBlobClient> clientMoq;

        private void SetupAppSettings(AppSettings appSettings)
        {
            appSettingsMoq = new Mock<IOptions<AppSettings>>();
            appSettingsMoq.Setup(p => p.Value).Returns(appSettings);
        }

        private StorageCredentials GetFakeCreds()
        {
            return new StorageCredentials("Fake",
             Convert.ToBase64String(Encoding.Unicode.GetBytes("fake")),
              "fakekey");
        }

        [TearDown]
        public void TearDown()
        {
            storageMoq = null;
            clientMoq = null;
            azureProviderMoq = null;
            appSettingsMoq = null;

        }

        [Test]
        public void MoqAzureProvider_GetStorageAccount()
        {
            StorageCredentials fakeCreds = GetFakeCreds();

            storageMoq = new Mock<CloudStorageAccount>(MockBehavior.Strict, fakeCreds, false);

            azureProviderMoq = new Mock<IAzureProvider>();                             

            azureProviderMoq.SetupGet(p => p.StorageAccount).Returns(storageMoq.Object);
            
            Assert.AreEqual(azureProviderMoq.Object.StorageAccount, storageMoq.Object);
        }

        [Test]
        public void MoqAzureProvider_GetClient()
        {
            StorageCredentials fakeCreds = GetFakeCreds();

            clientMoq = new Mock<CloudBlobClient>(MockBehavior.Strict, new Uri("http://fake.com"), fakeCreds);

            azureProviderMoq = new Mock<IAzureProvider>();       
                     

            azureProviderMoq.SetupGet(p => p.Client).Returns(clientMoq.Object);
            
            Assert.AreEqual(azureProviderMoq.Object.Client, clientMoq.Object);
        }

        [Test]
        public void MoqAzureProvider_AppSettingsIsNull()
        {
            azureProviderMoq = new Mock<IAzureProvider>();
            // Set up the azureProviderMoq CreateAccount method so that it throws an exception due to no config being set
            azureProviderMoq.Setup(p => p.Initialize()).Throws(new NullReferenceException("No App settings"));

            // Make method call and get exception
            var ex = Assert.Throws<NullReferenceException>(() => azureProviderMoq.Object.Initialize());

            // Assert that exception message is correct.
            Assert.That(ex.Message == "No App settings");
        }

        [Test]
        public void MoqAzureProvider_InvalidConnectionString()
        {
            AppSettings appSettings = new AppSettings()
            {
                ConnectionString = "fake"
            };

            SetupAppSettings(appSettings);

            azureProviderMoq = new Mock<IAzureProvider>();
            // Set up the azureProviderMoq CreateAccount method so that it throws an exception due to no config being set
            azureProviderMoq.Setup(p => p.Initialize()).Throws(new Exception("Error with account"));

            // Make method call and get exception
            var ex = Assert.Throws<Exception>(() => azureProviderMoq.Object.Initialize());

            // Assert that exception message is correct.
            Assert.That(ex.Message == "Error with account");
        }
    }
}
