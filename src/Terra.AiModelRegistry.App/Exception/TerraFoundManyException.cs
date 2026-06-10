namespace Terra.AiModelRegistry.App.Exception
{
	public class TerraFoundManyException : System.Exception
	{
		public int Code { get; set; }

		public TerraFoundManyException() : base() { }
		public TerraFoundManyException(int code) : this() { this.Code = code; }
		public TerraFoundManyException(String message) : base(message) { }
		public TerraFoundManyException(int code, String message) : this(message) { this.Code = code; }
		public TerraFoundManyException(String message, System.Exception innerException) : base(message, innerException) { }
		public TerraFoundManyException(int code, String message, System.Exception innerException) : this(message, innerException) { this.Code = code; }
	}
}
