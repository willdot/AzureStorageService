using System;
using NUnit;
using NUnit.Framework;
using Moq;

using BlobStorageService.Service;
using Microsoft.Extensions.Options;

namespace BlobStorageService.Tests
{
    [TestFixture]
    public class AzureProviderTests
    {
        IOptions<AppSettings> settings;
        Mock<IOptions<AppSettings>> appSettingsMoq;

        Mock<IAzureProvider> azureProviderMoq;

        private void SetupAppSettings(AppSettings appSettings)
        {
            appSettingsMoq = new Mock<IOptions<AppSettings>>();
            appSettingsMoq.Setup(p => p.Value).Returns(appSettings);
            settings = appSettingsMoq.Object;
        }

        [Test]
        public void TestMethod1()
        {
            AppSettings appSettings = new AppSettings()
            {
                ConnectionString = ""
            };

            SetupAppSettings(appSettings);



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
