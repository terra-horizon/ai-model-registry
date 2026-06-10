
namespace Terra.AiModelRegistry.App.Exception
{
	public class TerraUnauthorizedException : System.Exception
	{
		public int Code { get; set; }

		public TerraUnauthorizedException() : base() { }
		public TerraUnauthorizedException(int code) : this() { this.Code = code; }
		public TerraUnauthorizedException(String message) : base(message) { }
		public TerraUnauthorizedException(int code, String message) : this(message) { this.Code = code; }
		public TerraUnauthorizedException(String message, System.Exception innerException) : base(message, innerException) { }
		public TerraUnauthorizedException(int code, String message, System.Exception innerException) : this(message, innerException) { this.Code = code; }
	}
}
