using System.ComponentModel;

namespace Terra.AiModelRegistry.App.Common
{
	public enum ModelReferenceKind : short
	{
		[Description("Data is accessible via an HTTP or HTTPS endpoint.")]
		Http = 1,
		[Description("Data is accessible via an S3-compatible storage server.")]
		S3 = 2,
	}
}
