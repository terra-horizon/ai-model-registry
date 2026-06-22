using Cite.Tools.Time;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Terra.AiModelRegistry.App.Service.Convention
{
	public class ConventionService : IConventionService
	{
		private const String LogTrackingHeaderValue = "x-log-tracking";

		public ConventionService()
		{
		}

		public Boolean IsValidId(int? id)
		{
			return id.HasValue && id.Value > 0;
		}

		public Boolean IsValidGuid(Guid? guid)
		{
			return guid.HasValue && guid.Value != Guid.Empty;
		}

		public Boolean IsValidHash(String hash)
		{
			return !String.IsNullOrEmpty(hash);
		}

		//GOTCHA: This will only cover conflicts with a range up to a second. To go bellow that consider using datetime2 (sql server) or a different conflict resolution technique
		public String HashValue(Object value)
		{
			if (value == null) return String.Empty;
			if (value is DateTime) return ((DateTime)value).ToEpoch().ToString();
			//GOTCHA: If it reached this. probably will not work correctly
			return value.GetHashCode().ToString();
		}

		public String Limit(String text, int maxLength)
		{
			if (String.IsNullOrEmpty(text)) return text;
			if (text.Length > maxLength) return String.Format("{0}...", text.Substring(0, maxLength - 3));
			else return text;
		}

		public String Truncate(String text, int maxLength)
		{
			string truncated = text;
			if (text.Length < maxLength) return text;

			truncated = truncated.Trim();
			truncated = Regex.Replace(truncated, @"\s+", " ");//remove multiple spaces
			if (truncated.Length < maxLength) return truncated;
			truncated = Regex.Replace(truncated, @"([.!@#$%^&-=':;,<>?*\\""/|])+", "");//remove special chars
			if (truncated.Length < maxLength) return truncated;
			truncated = Regex.Replace(truncated, @"([aeiou])+", ""); //remove non capital vowel letters
			if (truncated.Length < maxLength) return truncated;
			truncated = Regex.Replace(truncated, @"([AEIOU])+", ""); //remove capital vowel letters
			if (truncated.Length < maxLength) return truncated;

			if (text.Length > maxLength) return String.Format("{0}...", text.Substring(0, maxLength));
			return text;
		}

		public String LogTrackingHeader()
		{
			return LogTrackingHeaderValue;
		}

		public Boolean IsNullOrValidInteger(String value)
		{
			return value == null || int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out _);
		}

		public Int32? AsInteger(String value)
		{
			if (String.IsNullOrEmpty(value)) return null;
			if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int result)) return result;
			return null;
		}

		public Boolean IsNullOrValidDecimal(String value)
		{
			return value == null || decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out _);
		}

		public Decimal? AsDecimal(String value)
		{
			if (String.IsNullOrEmpty(value)) return null;
			if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal result)) return result;
			return null;
		}

		public Boolean IsNullOrValidTimeSpan(String value)
		{
			return value == null || TimeSpan.TryParse(value, CultureInfo.InvariantCulture, out _);
		}

		public TimeSpan? AsTimeSpan(String value)
		{
			if (String.IsNullOrEmpty(value)) return null;
			if (TimeSpan.TryParse(value, CultureInfo.InvariantCulture, out TimeSpan result)) return result;
			return null;
		}

		public Boolean IsNullOrValidDate(String value)
		{
			return value == null || DateOnly.TryParseExact(value, ["yyyy/MM/dd"], out DateOnly _);
		}

		public DateOnly? AsDate(String value)
		{
			if (String.IsNullOrEmpty(value)) return null;
			if (DateOnly.TryParse(value, out DateOnly result)) return result;
			return null;
		}

		public bool IsNullOrValidYearMonth(string value)
		{
			return value == null || DateOnly.TryParseExact(value, ["yyyy/MM"], out DateOnly _);
		}

		public DateOnly? AsYearMonthOnly(string value)
		{
			DateOnly? result = this.AsDate(value);
			return result != null ? new DateOnly(result.Value.Year, result.Value.Month, 1) : result;
		}
	}
}
