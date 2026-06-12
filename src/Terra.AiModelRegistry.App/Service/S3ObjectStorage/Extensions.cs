using Amazon.Runtime;
using Amazon.S3;
using Cite.Tools.Configuration.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Terra.AiModelRegistry.App.Service.S3ObjectStorage
{
	public static class Extensions
	{
		public static IServiceCollection AddS3ObjectStorageServices(this IServiceCollection services, IConfigurationSection s3ObjectStorageSection)
		{
			string endpoint = s3ObjectStorageSection.GetSection("Endpoint").Get<string>();
			string accessKey = s3ObjectStorageSection.GetSection("AccessKey").Get<string>();
			string secretKey = s3ObjectStorageSection.GetSection("SecretKey").Get<string>();
			string region = s3ObjectStorageSection.GetSection("Region").Get<string>();
			services.ConfigurePOCO<S3ObjectStorageConfig>(s3ObjectStorageSection);

			services.AddSingleton<IAmazonS3>(x =>
			{
				AmazonS3Config s3Config = new AmazonS3Config
				{
					ServiceURL = endpoint,
					ForcePathStyle = true,
					AuthenticationRegion = region
				};
				BasicAWSCredentials credentials = new BasicAWSCredentials(accessKey, secretKey);
				return new AmazonS3Client(credentials, s3Config);
			});

			services.AddScoped<IS3ObjectStorage, MinioStorage>();

			return services;
		}
	}
}
