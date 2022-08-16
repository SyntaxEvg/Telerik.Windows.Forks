using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Printing
{
	public class PrintingException : LocalizableException
	{
		public PrintingException()
		{
		}

		public PrintingException(string message)
			: base(message)
		{
		}

		public PrintingException(string message, string key)
			: base(message, key, null)
		{
		}

		public PrintingException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public PrintingException(string message, Exception innerException, string key)
			: base(message, innerException, key, null)
		{
		}
	}
}
