using Cite.Tools.Json;
using Terra.AiModelRegistry.App.Exception;
using System.Net;

namespace Terra.AiModelRegistry.Api.Exception
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly JsonHandlingService _jsonHandlingService;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(
            RequestDelegate next,
            JsonHandlingService jsonHandlingService,
            ILogger<ErrorHandlingMiddleware> logger)
        {
            this.next = next;
            _jsonHandlingService = jsonHandlingService;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (System.Exception ex)
            {
                await HandleAsync(context, ex);
            }
        }

        private async Task HandleAsync(HttpContext context, System.Exception exception)
        {
            HandledException handled = HandleException(context, exception);

            _logger.Log(handled.Level, exception, "returning code {code} and payload {seralization}", handled.StatusCode, handled.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)handled.StatusCode;
            await context.Response.WriteAsync(handled.Message);
        }

        protected virtual HandledException HandleException(HttpContext context, System.Exception exception)
        {
            HttpStatusCode statusCode;
            object result;
            LogLevel logLevel;

            if (exception is TerraNotFoundException)
            {
                logLevel = LogLevel.Debug;
                statusCode = HttpStatusCode.NotFound;

                int code = ((TerraNotFoundException)exception).Code;
                if (code > 0) result = new { code, error = exception.Message };
                else result = new { error = exception.Message };
            }
            else if (exception is TerraForbiddenException)
            {
                logLevel = LogLevel.Debug;
                statusCode = HttpStatusCode.Forbidden;

                int code = ((TerraForbiddenException)exception).Code;
                if (code > 0) result = new { code, error = exception.Message };
                else result = new { error = exception.Message };
            }
			else if (exception is TerraUnauthorizedException)
			{
				logLevel = LogLevel.Debug;
				statusCode = HttpStatusCode.Unauthorized;

				int code = ((TerraUnauthorizedException)exception).Code;
				if (code > 0) result = new { code, error = exception.Message };
				else result = new { error = exception.Message };
			}
			else if (exception is TerraUnderpinningException)
			{
				logLevel = LogLevel.Error;
				statusCode = HttpStatusCode.FailedDependency;

                var payload = new
                {
                    statusCode = ((TerraUnderpinningException)exception).ErrorStatusCode,
                    source = ((TerraUnderpinningException)exception).ErrorSource?.ToString(),
                    correlationId = ((TerraUnderpinningException)exception).CorrelationId,
                    payload = ((TerraUnderpinningException)exception).ResponsePayload,
				};

				int code = ((TerraUnderpinningException)exception).Code;
				if (code > 0) result = new { code, error = exception.Message, message = payload };
				else result = new { error = exception.Message, message = payload };
			}
			else if (exception is TerraValidationException)
            {
                logLevel = LogLevel.Debug;
                statusCode = HttpStatusCode.BadRequest;

                int code = ((TerraValidationException)exception).Code;
                if (code > 0) result = new { code, error = exception.Message, message = ((TerraValidationException)exception).Errors };
                else result = new { error = exception.Message, message = ((TerraValidationException)exception).Errors };
            }
            else if (exception is TerraApplicationException)
            {
                logLevel = LogLevel.Error;
                statusCode = HttpStatusCode.InternalServerError;

                int code = ((TerraApplicationException)exception).Code;
                if (code > 0) result = new { code, error = exception.Message };
                else result = new { error = exception.Message };
            }
            else
            {
                logLevel = LogLevel.Error;
                statusCode = HttpStatusCode.InternalServerError;

                string message = "System error";

                result = new { error = message };
            }

            string serialization = _jsonHandlingService.ToJsonSafe(result);

            return new HandledException() { StatusCode = statusCode, Message = serialization, Level = logLevel };
        }

        public class HandledException
        {
            public HttpStatusCode StatusCode { get; set; }
            public string Message { get; set; }
            public LogLevel Level { get; set; }
        }
    }
}
