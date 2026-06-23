using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Terra.AiModelRegistry.App.Common;
using Terra.AiModelRegistry.App.ErrorCode;
using Terra.AiModelRegistry.App.Event;
using Terra.AiModelRegistry.App.Exception;
using Terra.AiModelRegistry.App.LogTracking;

namespace Terra.AiModelRegistry.App.Service.S3ObjectStorage
{
    public class MinioStorage : IS3ObjectStorage
    {
        private readonly IAmazonS3 _client;
        private readonly S3ObjectStorageConfig _config;
        private readonly LogTrackingCorrelationConfig _logTrackingCorrelationConfig;
        private readonly LogCorrelationScope _logCorrelationScope;
        private readonly ILogger<MinioStorage> _logger;
        private readonly ErrorThesaurus _errors;
        private readonly EventBroker _eventBroker;
        private readonly IStringLocalizer<Terra.AiModelRegistry.Resources.MySharedResources> _localizer;

        public MinioStorage(IAmazonS3 client,
            S3ObjectStorageConfig config,
            LogTrackingCorrelationConfig logTrackingCorrelationConfig,
            LogCorrelationScope logCorrelationScope,
            ILogger<MinioStorage> logger,
            ErrorThesaurus errors,
            EventBroker eventBroker,
            IStringLocalizer<Terra.AiModelRegistry.Resources.MySharedResources> localizer)
        {
            this._client = client;
            this._config = config;
            this._logTrackingCorrelationConfig = logTrackingCorrelationConfig;
            this._logCorrelationScope = logCorrelationScope;
            this._logger = logger;
            this._errors = errors;
            this._eventBroker = eventBroker;
            this._localizer = localizer;
        }

        public async Task<Stream> DownloadAsync(string objectName)
        {
            GetObjectResponse response = null;
            try
            {
                response = await this._client.GetObjectAsync(this._config.BucketName, objectName);
            }
            catch (System.Exception ex)
            {
                this._logger.LogError(ex, "Failed to download object {ObjectName} from bucket {BucketName}", objectName, this._config.BucketName);
                throw new TerraUnderpinningException(this._errors.UnderpinningService.Code, this._errors.UnderpinningService.Message, (int?)response?.HttpStatusCode, UnderpinningServiceType.S3ObjectStorage, this._logCorrelationScope.CorrelationId);
            }
            return response.ResponseStream;
        }

        public async Task<string> UploadAsync(Stream stream, string objectName, string contentType)
        {
            PutObjectRequest request = new PutObjectRequest
            {
                BucketName = this._config.BucketName,
                Key = objectName,
                InputStream = stream,
                ContentType = contentType,
            };

            PutObjectResponse response = null;
            try
            {
                response = await this._client.PutObjectAsync(request);
            }
            catch (System.Exception ex)
            {
                this._logger.LogError(ex, "Failed to upload object {ObjectName} to bucket {BucketName}", objectName, this._config.BucketName);
                throw new TerraUnderpinningException(this._errors.UnderpinningService.Code, this._errors.UnderpinningService.Message, (int?)response?.HttpStatusCode, UnderpinningServiceType.S3ObjectStorage, this._logCorrelationScope.CorrelationId);
            }
            return Path.Combine(this._config.Endpoint, this._config.BucketName, objectName);
		}

    }
}
