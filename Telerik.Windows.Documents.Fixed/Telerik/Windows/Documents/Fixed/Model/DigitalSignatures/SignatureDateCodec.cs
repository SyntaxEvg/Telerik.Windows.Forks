using System;
using System.Globalization;

namespace Telerik.Windows.Documents.Fixed.Model.DigitalSignatures
{
	static class SignatureDateCodec
	{
		public static DateTime? Decode(string inputDate)
		{
			DateTimeStyles style = DateTimeStyles.None;
			string format = "yyyyMMddHHmmsszzz";
			string text = inputDate;
			if (text.StartsWith("D:"))
			{
				text = text.Substring(2);
			}
			if (text.EndsWith("Z"))
			{
				text = text.Substring(0, text.Length - 1);
				format = "yyyyMMddHHmmss";
				style = DateTimeStyles.AssumeUniversal;
			}
			else
			{
				text = text.Replace("'", ":");
				text = text.Substring(0, text.Length - 1);
			}
			DateTime dateTime;
			if (!DateTime.TryParseExact(text, format, DateTimeFormatInfo.InvariantInfo, style, out dateTime))
			{
				return null;
			}
			DateTime value = dateTime.ToLocalTime();
			return new DateTime?(value);
		}

		internal static string Encode(DateTime date)
		{
			string format = "\\D\\:yyyyMMddHHmmss";
			string text = date.ToString(format, DateTimeFormatInfo.InvariantInfo);
			if (date.Kind == DateTimeKind.Local)
			{
				string text2 = date.ToString("zzz", DateTimeFormatInfo.InvariantInfo);
				text2 = text2.Replace(":", "'");
				text = text + text2 + "'";
			}
			else
			{
				text += "Z";
			}
			return text;
		}
	}
}
