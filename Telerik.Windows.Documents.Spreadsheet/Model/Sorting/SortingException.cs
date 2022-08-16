using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Sorting
{
	public class SortingException : LocalizableException
	{
		public SortingException()
		{
		}

		public SortingException(string message)
			: base(message)
		{
		}

		public SortingException(string message, string key)
			: base(message, key, null)
		{
		}

		public SortingException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public SortingException(string message, Exception innerException, string key)
			: base(message, innerException, key, null)
		{
		}
	}
}
