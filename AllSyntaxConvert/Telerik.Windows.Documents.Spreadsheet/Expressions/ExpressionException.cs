using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	public class ExpressionException : LocalizableException
	{
		public ExpressionException(string message)
			: base(message)
		{
		}

		public ExpressionException(string message, string key, string[] formatStringArguments = null)
			: base(message, key, formatStringArguments)
		{
		}

		public ExpressionException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public ExpressionException(string message, Exception innerException, string key, string[] formatStringArguments = null)
			: base(message, innerException, key, formatStringArguments)
		{
		}
	}
}
