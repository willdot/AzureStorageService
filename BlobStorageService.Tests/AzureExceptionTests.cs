using System;
using BlobStorageService.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Moq;
using NUnit;
using NUnit.Framework;

namespace BlobStorageService.Tests
{
    [TestFixture]
    public class AzureExceptionTests
    {
        private IConfiguration config;

        Mock<IOptions<AppSettings>> appSettingsMoq;

        AzureProvider provider;

        BlobStorageTools blobStorageTools;

        string sourceContainerReference;
        string destinationContainerReference;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            config = new ConfigurationBuilder()
            .AddJsonFile(Environment.CurrentDirectory + "../../../../appsettings.test.json")
            .Build();

            AppSettings appSettings = new AppSettings()
            {
                ConnectionString = config["ConnectionString"]
            };

            appSettingsMoq = new Mock<IOptions<AppSettings>>();
            appSettingsMoq.Setup(p => p.Value).Returns(appSettings);

            provider = new AzureProvider(appSettingsMoq.Object);
            provider.Initialize();
        }

        [SetUp]
        public void Setup()
        {
            blobStorageTools = new BlobStorageTools(provider);
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                blobStorageTools.DeleteContainer(sourceContainerReference);
            }
            catch (Exception)
            {
                //
            }

            try
            {
                blobStorageTools.DeleteContainer(destinationContainerReference);
            }
            catch (Exception)
            {
                //
            }

            blobStorageTools = null;
            sourceContainerReference = null;
            destinationContainerReference = null;
        }


        [Test]
        public void Move_BlobNotFoundException()
        {
            string exceptionMessage = "";

            sourceContainerReference = Guid.NewGuid().ToString();
            destinationContainerReference = Guid.NewGuid().ToString();
            blobStorageTools.CreateContainer(sourceContainerReference);
            blobStorageTools.CreateContainer(destinationContainerReference);

            try
            {
                blobStorageTools.Move(sourceContainerReference, "fake1", destinationContainerReference, "fake1");
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            Assert.AreEqual(exceptionMessage, "The specified blob does not exist.");
        }

        [Test]
        public void Move_ContainerNotFoundException()
        {
            string exceptionMessage = "";

            sourceContainerReference = Guid.NewGuid().ToString();
            destinationContainerReference = Guid.NewGuid().ToString();

            try
            {
                blobStorageTools.Move(sourceContainerReference, "fake1", destinationContainerReference, "fake1");
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            Assert.AreEqual(exceptionMessage, "The specified container does not exist.");
        }

        [Test]
        public void Copy_BlobNotFoundException()
        {
            string exceptionMessage = "";

            sourceContainerReference = Guid.NewGuid().ToString();
            destinationContainerReference = Guid.NewGuid().ToString();
            blobStorageTools.CreateContainer(sourceContainerReference);
            blobStorageTools.CreateContainer(destinationContainerReference);

            try
            {
                blobStorageTools.Copy(sourceContainerReference, "fake2", sourceContainerReference, "fake2");
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            Assert.AreEqual(exceptionMessage, "The specified blob does not exist.");
        }

        [Test]
        public void Copy_ContainerNotFoundException()
        {
            string exceptionMessage = "";

            sourceContainerReference = Guid.NewGuid().ToString();
            destinationContainerReference = Guid.NewGuid().ToString();

            try
            {
                blobStorageTools.Copy(sourceContainerReference, "fake2", destinationContainerReference, "fake2");
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            Assert.AreEqual(exceptionMessage, "The specified container does not exist.");
        }

        [Test]
        public void Delete_ContainerNotFoundException()
        {
            string exceptionMessage = "";
            sourceContainerReference = Guid.NewGuid().ToString();

            try
            {
                blobStorageTools.Delete(sourceContainerReference, "fake3");
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            Assert.AreEqual(exceptionMessage, "The specified container does not exist.");
        }
    }
}