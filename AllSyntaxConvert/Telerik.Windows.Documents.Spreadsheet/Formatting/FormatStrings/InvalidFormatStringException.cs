using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings
{
	public class InvalidFormatStringException : LocalizableException
	{
		public InvalidFormatStringException()
			: base("Invalid Formatting String.", "Spreadsheet_ErrorExpressions_FormatStringInvalid", null)
		{
		}
	}
}
