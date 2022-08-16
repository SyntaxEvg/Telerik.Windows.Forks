using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class SpreadsheetNameException : LocalizableException
	{
		public SpreadsheetNameException(string message)
			: base(message)
		{
		}

		public SpreadsheetNameException(string message, string key)
			: base(message, key, null)
		{
		}

		public SpreadsheetNameException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public SpreadsheetNameException(string message, Exception innerException, string key)
			: base(message, innerException, key, null)
		{
		}
	}
}
