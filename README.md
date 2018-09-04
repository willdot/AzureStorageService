# AzureStorageService

## Overview

This library is an easy way to set up a service to work with Azure Blob Storage. 

## Basic Usage

The library can be used from within a .Net Core application.

### Initialize in Startup

In your appsettings.json file, add a "ConnectionString" that has a value of your Azure blob storage connection.

Connection string on Azure can be found in the Access Keys section of your storage account overview.

``` JSON
// appsettings.json
{
  "ConnectionString":"",
}
```

``` C#
// startup.cs
public Startup(IConfiguration configuration)
{
  Configuration = configuration;
}

public IConfiguration Configuration { get; }

// This method gets called by the runtime. Use this method to add services to the container.
public void ConfigureServices(IServiceCollection services)
{

    services.Configure<AppSettings>(Configuration);

    ServiceSetup.SetupService(services, Configuration);
   
}
```

### Use the service

To use the service, add a dependancy in your class constructor for an IAzureProvider, which will be supplied by the dependancy injection system, as configured above.

Create a new instance of a BlobStorageTools, parsing in the IAzureProvider. Then call the methods you require.

``` C#
public class Test
{
    private readonly IAzureProvider Provider;
    public Test(IAzureProvider _provider)
    {
        Provider = _provider;
        Provider.Initialize();
    }
    
    public async Task DoSomething()
    {
        BlobStorageTools tools = new BlobStorageTools(Provider);
        
        await tools.UploadAsync("container reference", "File path to upload");
        
        await tools.DownloadAsync("container reference", "folder path to save to", "filename to download");

        await tools.MoveAsync("source container reference", "source file name",  "destination container reference", "destination file name");

        await tools.CopyAsync("source container reference", "source file name",  "destination container reference", "destination file name");

        await tools.DeleteAsync("container reference", "file name");

        await tools.CreateContainerAsync("container reference");

        await tools.DeleteContainterAsync("container reference");
    }
}
```

A demo can be found in the BlobStorageService.Demo project.
