
namespace Terra.AiModelRegistry.App.Exception
{
	public class TerraApplicationException : System.Exception
	{
		public int Code { get; set; }

		public TerraApplicationException() : base() { }
		public TerraApplicationException(int code) : this() { this.Code = code; }
		public TerraApplicationException(String message) : base(message) { }
		public TerraApplicationException(int code, String message) : this(message) { this.Code = code; }
		public TerraApplicationException(String message, System.Exception innerException) : base(message, innerException) { }
		public TerraApplicationException(int code, String message, System.Exception innerException) : this(message, innerException) { this.Code = code; }
	}
}
