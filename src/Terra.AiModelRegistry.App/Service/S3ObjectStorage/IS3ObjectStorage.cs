namespace Terra.AiModelRegistry.App.Service.S3ObjectStorage
{
	public interface IS3ObjectStorage
	{
		Task UploadAsync(Stream stream, string objectName, string contentType);

		Task<Stream> DownloadAsync(string objectName);
	}
}
