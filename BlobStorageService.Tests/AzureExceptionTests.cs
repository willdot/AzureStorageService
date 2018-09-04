using System;
using System.Threading.Tasks;
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
        public async Task TearDown()
        {
            try
            {
                await blobStorageTools.DeleteContainerAsync(sourceContainerReference);
            }
            catch (Exception)
            {
                //
            }

            try
            {
                await blobStorageTools.DeleteContainerAsync(destinationContainerReference);
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
        public async Task Move_BlobNotFoundException()
        {
            string actualExceptionMessage = "";
            string expectedExceptionMessage = "The specified blob does not exist.";

            sourceContainerReference = Guid.NewGuid().ToString();
            destinationContainerReference = Guid.NewGuid().ToString();
            await blobStorageTools.CreateContainerAsync(sourceContainerReference);
            await blobStorageTools.CreateContainerAsync(destinationContainerReference);

            try
            {
                await blobStorageTools.MoveAsync(sourceContainerReference, "fake", destinationContainerReference, "fake");
            }
            catch (Exception ex)
            {
                actualExceptionMessage = ex.Message;
            }

            Assert.AreEqual(expectedExceptionMessage, actualExceptionMessage);
        }

        [Test]
        public async Task Move_ContainerNotFoundException()
        {
            string actualExceptionMessage = "";
            string expectedExceptionMessage = "The specified container does not exist.";

            sourceContainerReference = Guid.NewGuid().ToString();
            destinationContainerReference = Guid.NewGuid().ToString();

            try
            {
                await blobStorageTools.MoveAsync(sourceContainerReference, "fake", destinationContainerReference, "fake");
            }
            catch (Exception ex)
            {
                actualExceptionMessage = ex.Message;
            }

            Assert.AreEqual(expectedExceptionMessage ,actualExceptionMessage);
        }

        [Test]
        public async Task Copy_BlobNotFoundException()
        {
            string actualExceptionMessage = "";
            string expectedExceptionMessage = "The specified blob does not exist.";

            sourceContainerReference = Guid.NewGuid().ToString();
            destinationContainerReference = Guid.NewGuid().ToString();
            await blobStorageTools.CreateContainerAsync(sourceContainerReference);
            await blobStorageTools.CreateContainerAsync(destinationContainerReference);

            try
            {
                await blobStorageTools.CopyAsync(sourceContainerReference, "fake", sourceContainerReference, "fake");
            }
            catch (Exception ex)
            {
                actualExceptionMessage = ex.Message;
            }

            Assert.AreEqual(expectedExceptionMessage ,actualExceptionMessage);
        }

        [Test]
        public async Task Copy_ContainerNotFoundException()
        {
            string actualExceptionMessage = "";
            string expectedExceptionMessage = "The specified container does not exist.";

            sourceContainerReference = Guid.NewGuid().ToString();
            destinationContainerReference = Guid.NewGuid().ToString();

            try
            {
                await blobStorageTools.CopyAsync(sourceContainerReference, "fake", destinationContainerReference, "fake");
            }
            catch (Exception ex)
            {
                actualExceptionMessage = ex.Message;
            }

            Assert.AreEqual(expectedExceptionMessage ,actualExceptionMessage);
        }

        [Test]
        public async Task DeleteFile_ContainerNotFoundException()
        {
            string actualExceptionMessage = "";
            string expectedExceptionMessage = "The specified container does not exist.";

            sourceContainerReference = Guid.NewGuid().ToString();

            try
            {
                await blobStorageTools.DeleteAsync(sourceContainerReference, "fake");
            }
            catch (Exception ex)
            {
                actualExceptionMessage = ex.Message;
            }

            Assert.AreEqual(expectedExceptionMessage ,actualExceptionMessage);
        }

        [Test]
        public async Task DeleteFile_BlobNotFoundExeption()
        {
            string actualExceptionMessage = "";
            string expectedExceptionMessage = "The specified blob does not exist.";

            sourceContainerReference = Guid.NewGuid().ToString();

            await blobStorageTools.CreateContainerAsync(sourceContainerReference);

            try
            {
                await blobStorageTools.DeleteAsync(sourceContainerReference, "fake");
            }
            catch (Exception ex)
            {
                actualExceptionMessage = ex.Message;
            }

            Assert.AreEqual(expectedExceptionMessage, actualExceptionMessage);
        }

        [Test]
        public async Task DownloadFile_BlobNotFoundExeption()
        {
            string actualExceptionMessage = "";
            string expectedExceptionMessage = "The specified blob does not exist.";

            sourceContainerReference = Guid.NewGuid().ToString();

            await blobStorageTools.CreateContainerAsync(sourceContainerReference);

            try
            {
                await blobStorageTools.DownloadAsync(sourceContainerReference, "fake", "fake");
            }
            catch (Exception ex)
            {
                actualExceptionMessage = ex.Message;
            }

            Assert.AreEqual(expectedExceptionMessage, actualExceptionMessage);
        }

        [Test]
        public async Task DownloadFile_ContainerNotFoundExeption()
        {
            string actualExceptionMessage = "";
            string expectedExceptionMessage = "The specified container does not exist.";

            sourceContainerReference = Guid.NewGuid().ToString();

            try
            {
                await blobStorageTools.DownloadAsync(sourceContainerReference, "fake", "fake");
            }
            catch (Exception ex)
            {
                actualExceptionMessage = ex.Message;
            }

            Assert.AreEqual(expectedExceptionMessage, actualExceptionMessage);
        }

        // This test needs to upload a file, then download it but fail when looking for destination directory
        [Test]
        public async Task DownloadFile_DestinationDirectoryNotFoundExeption()
        {
            string actualExceptionMessage = "";
            string expectedExceptionMessage = "The path cannot be found";

            sourceContainerReference = Guid.NewGuid().ToString();

            await blobStorageTools.CreateContainerAsync(sourceContainerReference);
            await blobStorageTools.UploadAsync(sourceContainerReference, "../../../test.txt");
            
            try
            {
                await blobStorageTools.DownloadAsync(sourceContainerReference, "test.txt", "fakepath");
            }
            catch (Exception ex)
            {
                actualExceptionMessage = ex.Message;
            }

            Assert.AreEqual(expectedExceptionMessage, actualExceptionMessage);
        }

        [Test]
        public async Task UploadFile_ContainerNotFoundExeption()
        {
            string actualExceptionMessage = "";
            string expectedExceptionMessage = "The specified container does not exist.";

            sourceContainerReference = Guid.NewGuid().ToString();

            try
            {
                await blobStorageTools.UploadAsync(sourceContainerReference, "../../../test.txt");
            }
            catch (Exception ex)
            {
                actualExceptionMessage = ex.Message;
            }

            Assert.AreEqual(expectedExceptionMessage, actualExceptionMessage);
        }

        [Test]
        public async Task UploadFile_FileNotFoundExeption()
        {
            string actualExceptionMessage = "";

            string directory = Environment.CurrentDirectory;
            string expectedExceptionMessage = $"Could not find file '{directory}\\fake.txt'.";

            sourceContainerReference = Guid.NewGuid().ToString();
            await blobStorageTools.CreateContainerAsync(sourceContainerReference);

            try
            {
                await blobStorageTools.UploadAsync(sourceContainerReference, "fake.txt");
            }
            catch (Exception ex)
            {
                actualExceptionMessage = ex.Message;
            }

            Assert.AreEqual(expectedExceptionMessage, actualExceptionMessage);
        }

        // Create container

        // Delete container
    }
}