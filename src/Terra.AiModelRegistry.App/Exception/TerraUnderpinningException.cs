
using Terra.AiModelRegistry.App.Common;

namespace Terra.AiModelRegistry.App.Exception
{
	public class TerraUnderpinningException : System.Exception
	{
		public int Code { get; set; }

		public int? ErrorStatusCode { get; set; }
		public UnderpinningServiceType? ErrorSource {  get; set; }
		public String CorrelationId { get; set; }
		public String ResponsePayload { get; set; }

		public TerraUnderpinningException() : base() { }
		public TerraUnderpinningException(int code) : this() { this.Code = code; }
		public TerraUnderpinningException(String message) : base(message) { }
		public TerraUnderpinningException(int code, String message) : this(message) { this.Code = code; }
		public TerraUnderpinningException(int code, String message, int? errorStatusCode = null, UnderpinningServiceType? errorSource = null, String correlationId = null, String responsePayload = null) : this(code, message) 
		{ 
			this.ErrorStatusCode = errorStatusCode;
			this.ErrorSource = errorSource;
			this.CorrelationId = correlationId;
			this.ResponsePayload = responsePayload;
		}
		public TerraUnderpinningException(String message, System.Exception innerException) : base(message, innerException) { }
		public TerraUnderpinningException(int code, String message, System.Exception innerException) : this(message, innerException) { this.Code = code; }
	}
}
