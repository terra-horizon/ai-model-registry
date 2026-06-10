
namespace Terra.AiModelRegistry.App.Exception
{
	public class TerraNotFoundException : System.Exception
	{
		public int Code { get; set; }

		public TerraNotFoundException() : base() { }
		public TerraNotFoundException(int code) : this() { this.Code = code; }
		public TerraNotFoundException(String message) : base(message) { }
		public TerraNotFoundException(int code, String message) : this(message) { this.Code = code; }
		public TerraNotFoundException(String message, System.Exception innerException) : base(message, innerException) { }
		public TerraNotFoundException(int code, String message, System.Exception innerException) : this(message, innerException) { this.Code = code; }
	}
}
