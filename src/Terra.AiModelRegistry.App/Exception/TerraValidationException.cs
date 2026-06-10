
namespace Terra.AiModelRegistry.App.Exception
{
	public class TerraValidationException : System.Exception
	{
		public List<KeyValuePair<String, List<String>>> Errors { get; set; }
		public int Code { get; set; }

		public TerraValidationException() : base("Validation Error") { }
		public TerraValidationException(int code) : this() { this.Code = code; }
		public TerraValidationException(String key, String error) : this() { this.Errors = new List<KeyValuePair<string, List<string>>>() { new KeyValuePair<string, List<string>>(key, new List<string>() { error }) }; }
		public TerraValidationException(int code, String key, String error) : this(key, error) { this.Code = code; }
		public TerraValidationException(KeyValuePair<String, List<String>> errors) : this() { this.Errors = new List<KeyValuePair<string, List<string>>>() { errors }; }
		public TerraValidationException(int code, KeyValuePair<String, List<String>> errors) : this(errors) { this.Code = code; }
		public TerraValidationException(List<KeyValuePair<String, List<String>>> errors) : this() { this.Errors = errors; }
		public TerraValidationException(int code, List<KeyValuePair<String, List<String>>> errors) : this(errors) { this.Code = code; }
		public TerraValidationException(String message) : base(message) { }
		public TerraValidationException(int code, String message) : this(message) { this.Code = code; }
		public TerraValidationException(String message, List<KeyValuePair<String, List<String>>> errors) : this(message) { this.Errors = errors; }
		public TerraValidationException(int code, String message, List<KeyValuePair<String, List<String>>> errors) : this(message, errors) { this.Code = code; }
		public TerraValidationException(String message, System.Exception innerException) : base(message, innerException) { }
		public TerraValidationException(int code, String message, System.Exception innerException) : this(message, innerException) { this.Code = code; }
		public TerraValidationException(String message, System.Exception innerException, List<KeyValuePair<String, List<String>>> errors) : this(message, innerException) { this.Errors = errors; }
		public TerraValidationException(int code, String message, System.Exception innerException, List<KeyValuePair<String, List<String>>> errors) : this(message, innerException, errors) { this.Code = code; }
	}
}
