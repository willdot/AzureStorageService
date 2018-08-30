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

        [Test]
        public void TestMethod1()
        {
            AppSettings appSettings = new AppSettings()
            {
                ConnectionString = ""
            };

            appSettingsMoq = new Mock<IOptions<AppSettings>>();
            appSettingsMoq.Setup(p => p.Value).Returns(appSettings);
            settings = appSettingsMoq.Object;



        }
    }
}
