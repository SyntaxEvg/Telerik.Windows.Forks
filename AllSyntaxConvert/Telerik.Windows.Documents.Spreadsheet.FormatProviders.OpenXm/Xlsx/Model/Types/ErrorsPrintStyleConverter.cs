using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Model.Printing;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types
{
	class ErrorsPrintStyleConverter : IStringConverter<ErrorsPrintStyle>
	{
		public ErrorsPrintStyle ConvertFromString(string str)
		{
			if (string.Equals(str, "Blank", StringComparison.OrdinalIgnoreCase))
			{
				return ErrorsPrintStyle.AsDisplayed;
			}
			if (string.Equals(str, "Dashes", StringComparison.OrdinalIgnoreCase))
			{
				return ErrorsPrintStyle.Dashes;
			}
			if (string.Equals(str, "NotAvailableError", StringComparison.OrdinalIgnoreCase))
			{
				return ErrorsPrintStyle.NotAvailableError;
			}
			return ErrorsPrintStyle.AsDisplayed;
		}

		public string ConvertToString(ErrorsPrintStyle errorsPrintStyle)
		{
			if (errorsPrintStyle == ErrorsPrintStyle.AsDisplayed)
			{
				return "displayed";
			}
			if (errorsPrintStyle == ErrorsPrintStyle.Blank)
			{
				return "blank";
			}
			if (errorsPrintStyle == ErrorsPrintStyle.Dashes)
			{
				return "dash";
			}
			return "NA";
		}
	}
}
