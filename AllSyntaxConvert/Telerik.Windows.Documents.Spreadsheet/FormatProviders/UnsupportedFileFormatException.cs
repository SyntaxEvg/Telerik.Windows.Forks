using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders
{
	public class UnsupportedFileFormatException : LocalizableException
	{
		public override string Message
		{
			get
			{
				return this.message;
			}
		}

		public UnsupportedFileFormatException(string extension)
			: base(string.Format("File format \"{0}\" is not supported.", extension), "Spreadsheet_ErrorExpressions_FormatNotSupported", new string[] { extension })
		{
			this.message = string.Format("File format \"{0}\" is not supported.", extension);
		}

		readonly string message;
	}
}
