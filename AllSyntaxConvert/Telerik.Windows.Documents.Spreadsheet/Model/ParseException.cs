using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class ParseException : LocalizableException
	{
		public ParseException(string message)
			: base(message)
		{
		}

		public ParseException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public ParseException(string message, Exception innerException, string key, string[] formatStringArguments)
			: base(message, innerException, key, formatStringArguments)
		{
		}
	}
}
