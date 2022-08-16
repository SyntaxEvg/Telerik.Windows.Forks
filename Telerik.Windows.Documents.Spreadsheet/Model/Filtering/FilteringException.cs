using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Filtering
{
	public class FilteringException : LocalizableException
	{
		public FilteringException()
		{
		}

		public FilteringException(string message)
			: base(message)
		{
		}

		public FilteringException(string message, string key)
			: base(message, key, null)
		{
		}

		public FilteringException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public FilteringException(string message, Exception innerException, string key)
			: base(message, innerException, key, null)
		{
		}
	}
}
