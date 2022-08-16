using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Protection
{
	public class ProtectionException : LocalizableException
	{
		public ProtectionException(string message)
			: base(message)
		{
		}

		public ProtectionException(string message, string key)
			: base(message, key, null)
		{
		}

		public ProtectionException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public ProtectionException(string message, Exception innerException, string key)
			: base(message, innerException, key, null)
		{
		}
	}
}
