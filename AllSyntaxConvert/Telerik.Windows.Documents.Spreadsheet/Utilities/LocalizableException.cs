using System;

namespace Telerik.Windows.Documents.Spreadsheet.Utilities
{
	public class LocalizableException : Exception
	{
		public string LocalizationKey
		{
			get
			{
				return this.localizationKey;
			}
			protected set
			{
				this.localizationKey = value;
			}
		}

		public string[] FormatStringArguments
		{
			get
			{
				return this.formatStringArguments;
			}
		}

		public LocalizableException()
		{
		}

		public LocalizableException(string message)
			: base(message)
		{
		}

		public LocalizableException(string message, string key, string[] formatStringArguments = null)
			: base(message)
		{
			this.localizationKey = key;
			if (formatStringArguments != null)
			{
				this.formatStringArguments = formatStringArguments;
			}
		}

		public LocalizableException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public LocalizableException(string message, Exception innerException, string key, string[] formatStringArguments = null)
			: base(message, innerException)
		{
			this.localizationKey = key;
			if (formatStringArguments != null)
			{
				this.formatStringArguments = formatStringArguments;
			}
		}

		string localizationKey;

		string[] formatStringArguments = new string[0];
	}
}
