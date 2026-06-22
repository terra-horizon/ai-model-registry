namespace Terra.AiModelRegistry.App.Service.Convention
{
	public interface IConventionService
	{
		DateOnly? AsDate(string value);
		decimal? AsDecimal(string value);
		int? AsInteger(string value);
		TimeSpan? AsTimeSpan(string value);
		DateOnly? AsYearMonthOnly(string value);
		string HashValue(object value);
		bool IsNullOrValidDate(string value);
		bool IsNullOrValidDecimal(string value);
		bool IsNullOrValidInteger(string value);
		bool IsNullOrValidTimeSpan(string value);
		bool IsNullOrValidYearMonth(string value);
		bool IsValidGuid(Guid? guid);
		bool IsValidHash(string hash);
		bool IsValidId(int? id);
		string Limit(string text, int maxLength);
		string LogTrackingHeader();
		string Truncate(string text, int maxLength);
	}
}