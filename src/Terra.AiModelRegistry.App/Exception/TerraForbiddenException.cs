
namespace Terra.AiModelRegistry.App.Exception
{
	public class TerraForbiddenException : System.Exception
	{
		public int Code { get; set; }

		public TerraForbiddenException() : base() { }
		public TerraForbiddenException(int code) : this() { this.Code = code; }
		public TerraForbiddenException(String message) : base(message) { }
		public TerraForbiddenException(int code, String message) : this(message) { this.Code = code; }
		public TerraForbiddenException(String message, System.Exception innerException) : base(message, innerException) { }
		public TerraForbiddenException(int code, String message, System.Exception innerException) : this(message, innerException) { this.Code = code; }
	}
}
