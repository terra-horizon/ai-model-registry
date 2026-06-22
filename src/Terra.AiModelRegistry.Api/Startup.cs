using Cite.Tools.Data.Builder.Extensions;
using Cite.Tools.Data.Censor.Extensions;
using Cite.Tools.Data.Deleter.Extensions;
using Cite.Tools.Data.Query.Extensions;
using Cite.Tools.Json;
using Cite.Tools.Logging.Extensions;
using Cite.Tools.Validation.Extensions;
using Cite.WebTools.Cors.Extensions;
using Cite.WebTools.CurrentPrincipal.Extensions;
using Cite.WebTools.FieldSet;
using Cite.WebTools.HostingEnvironment.Extensions;
using Cite.WebTools.Localization.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Terra.AiModelRegistry.Api.AccessToken;
using Terra.AiModelRegistry.Api.Authorization;
using Terra.AiModelRegistry.Api.Cache;
using Terra.AiModelRegistry.Api.Exception;
using Terra.AiModelRegistry.Api.ForwardedHeaders;
using Terra.AiModelRegistry.Api.HealthCheck;
using Terra.AiModelRegistry.Api.LogTracking;
using Terra.AiModelRegistry.Api.OpenApi;
using Terra.AiModelRegistry.Api.Transaction;
using Terra.AiModelRegistry.App.AccessToken;
using Terra.AiModelRegistry.App.Accounting;
using Terra.AiModelRegistry.App.ErrorCode;
using Terra.AiModelRegistry.App.Event;
using Terra.AiModelRegistry.App.Formatting;
using Terra.AiModelRegistry.App.LogTracking;
using Terra.AiModelRegistry.App.Service.AiModel;
using Terra.AiModelRegistry.App.Service.Convention;
using Terra.AiModelRegistry.App.Service.S3ObjectStorage;
using Terra.AiModelRegistry.App.Service.Version;

namespace Terra.AiModelRegistry.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration, IWebHostEnvironment env)
		{
			this._config = configuration;
			this._env = env;
		}

		private IConfiguration _config { get; }
		private IWebHostEnvironment _env { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services
				.AddHttpClient() //HttpClient for outgoing http calls
				.AddCacheServices(this._config.GetSection("Cache:Provider")) //distributed cache
				.AddSingleton<JsonHandlingService>() //Json Handling
				.AddErrorThesaurus(this._config.GetSection("ErrorThesaurus")) //Error Thesaurus
				.AddLocalization(options => options.ResourcesPath = this._config.GetSection("Localization:Path").Get<String>()) //Localization
				.AddCurrentPrincipalResolver() //Current principal Resolver
				.AddEventBroker() //Event Broker
				.AddFormattingServices(this._config.GetSection("Formatting:Options"), this._config.GetSection("Formatting:Cache")) //Formatting
				.AddClaimExtractorServices(this._config.GetSection("Idp:Claims")) //Claim Extractor
				.AddAuthenticationServices(this._config.GetSection("Idp:Client")) //Authentication & JWT
				.AddCorsPolicy(this._config.GetSection("CorsPolicy")) //CORS
				.AddForwardedHeadersServices(this._config.GetSection("ForwardedHeaders")) //Forwarded Headers
				.AddAspNetCoreHostingEnvironmentResolver() //Hosting Environment
				.AddLogTrackingServices(this._config.GetSection("Tracking:Correlation"), this._config.GetSection("Tracking:Principal")) //Log tracking services
				.AddPermissionsAndPolicies(this._config.GetSection("Permissions")) //Permissions
				.AddAuthorizationContentResolverServices() //Authorization Content Resolver
				.AddAccountingServices(this._config.GetSection("Accounting")) //Accounting
				.AddAccessTokenServices(); //Access token management services

			services
				.AddCensorsAndFactory(typeof(Cite.Tools.Data.Censor.ICensor), typeof(Terra.AiModelRegistry.App.AssemblyHandle)) //Censors
				.AddQueriesAndFactory(typeof(Cite.Tools.Data.Query.IQuery), typeof(Terra.AiModelRegistry.App.AssemblyHandle)) //Queries
				.AddBuildersAndFactory(typeof(Cite.Tools.Data.Builder.IBuilder), typeof(Terra.AiModelRegistry.App.AssemblyHandle)) //Builders
				.AddValidatorsAndFactory(typeof(Cite.Tools.Validation.IValidator), typeof(Terra.AiModelRegistry.App.AssemblyHandle), typeof(Terra.AiModelRegistry.Api.AssemblyHandle)) //Validators
				.AddDeletersAndFactory(typeof(Cite.Tools.Data.Deleter.IDeleter), typeof(Terra.AiModelRegistry.App.AssemblyHandle)) //Deleters
				.AddDbContext<Terra.AiModelRegistry.App.Data.AppDbContext>(options => options.UseNpgsql(this._config.GetValue<String>("DB:ConnectionStrings:AppDbContext"))) //DbContext
				.AddScoped<AppTransactionFilter>() //Transaction Filter
			;

			services
				.AddS3ObjectStorageServices(this._config.GetSection("S3ObjectStorage")) //S3 Object Storage Service
                .AddConventionServices() //Conventions
				.AddAiModelServices() //AI Model related services
                .AddScoped<IVersionInfoService, VersionInfoService>()
			;


			HealthCheckConfig healthCheckConfig = this._config.GetSection("HealthCheck").AsHealthCheckConfig();
			services.AddFolderHealthChecks(healthCheckConfig.Folder);
			services.AddMemoryHealthChecks(healthCheckConfig.Memory);
			services.AddDbHealthChecks<Terra.AiModelRegistry.App.Data.AppDbContext>();

			//Logging
			Cite.Tools.Logging.LoggingSerializerContractResolver.Instance.Configure((builder) =>
			{
				builder
					.RuntimeScannng(true);
					//.Sensitive(typeof(Cite.Tools.Http.HeaderHints), nameof(Cite.Tools.Http.HeaderHints.BearerAccessToken))
					//.Sensitive(typeof(Cite.Tools.Http.HeaderHints), nameof(Cite.Tools.Http.HeaderHints.BasicAuthenticationToken));
			}, (settings) =>
			{
				settings.Converters.Add(new Cite.Tools.Logging.StringValueEnumConverter());
			});

			//MVC
			services.AddMvcCore(options =>
			{
				options.ModelBinderProviders.Insert(0, new FieldSetModelBinderProvider());
			})
			.AddAuthorization()
			.AddNewtonsoftJson(options =>
			{
				options.SerializerSettings.Culture = System.Globalization.CultureInfo.InvariantCulture;
				options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
				options.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
				options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
			})
			.AddApiExplorer(); //needed because of Swashbuckle

			services.AddOpenApiServices(this._config.GetSection("OpenApi"));
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			HealthCheckConfig healthCheckConfig = this._config.GetSection("HealthCheck").AsHealthCheckConfig();

			app
				.UseMiddleware(typeof(LogTrackingCorrelationMiddleware)) //Log Tracking Middleware
				.UseSerilogRequestLogging() //Aggregated request info logging
				.UseForwardedHeaders(this._config.GetSection("ForwardedHeaders")) //Handle Forwarded Requests and preserve caller context
				.UseRequestLocalizationAndConfigure(this._config.GetSection("Localization:SupportedCultures"), this._config.GetSection("Localization:DefaultCulture")) //Request Localization
				.UseCorsPolicy(this._config.GetSection("CorsPolicy")) //CORS
				.UseMiddleware(typeof(ErrorHandlingMiddleware)) //Error Handling
				.UseRouting() //Routing
				.UseAuthentication() //Authentication
				.UseAuthorization() //Authorization
				.UseMiddleware(typeof(LogTrackingPrincipalMiddleware)) //Log Entry Middleware
				.UseMiddleware(typeof(AccessTokenInterceptMiddleware)) //Bearer Authorization AccessToken interception
				.UseEndpoints(endpoints => //Endpoints
				{
					endpoints.MapControllers();
					if (healthCheckConfig.Endpoint?.IsEnabled ?? false) endpoints.ConfigureHealthCheckEndpoint(healthCheckConfig.Endpoint);
				})
				.ConfigureUseSwagger(this._config.GetSection("OpenApi"), env.EnvironmentName);
		}
	}
}
