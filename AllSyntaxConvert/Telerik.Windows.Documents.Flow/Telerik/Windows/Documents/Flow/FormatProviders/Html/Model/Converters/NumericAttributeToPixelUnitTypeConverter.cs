using System;
using System.Linq;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters
{
	class NumericAttributeToPixelUnitTypeConverter : ILegacyConverter
	{
		public bool TryGetConvertedValue(string value, out string convertedValue)
		{
			Guard.ThrowExceptionIfNullOrEmpty(value, "value");
			convertedValue = string.Empty;
			string text = value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault<string>();
			if (NumericAttributeToPixelUnitTypeConverter.IsStringANumber(text))
			{
				convertedValue = text + "px";
				return true;
			}
			return false;
		}

		static bool IsStringANumber(string numberCandidate)
		{
			if (string.IsNullOrEmpty(numberCandidate))
			{
				return false;
			}
			bool result = true;
			foreach (char c in numberCandidate)
			{
				if (!char.IsDigit(c))
				{
					result = false;
					break;
				}
			}
			return result;
		}

		const string PixelUnitType = "px";
	}
}
