namespace BlobStorageService.Service
{
    public enum StorageErrorCode
    {
        None = 0,
        InvalidCredentials = 1000,
        GenericException = 1001,
        InvalidAccess = 1002,
        BlobInUse = 1003,
        InvalidName = 1005,
        ErrorOpeningBlob = 1006,
        NoCredentialsProvided = 1007,
        NotFound = 1008
    }
}