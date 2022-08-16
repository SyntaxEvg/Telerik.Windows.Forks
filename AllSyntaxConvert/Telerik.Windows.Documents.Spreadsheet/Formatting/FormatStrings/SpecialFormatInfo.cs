using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings
{
	public class SpecialFormatInfo
	{
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public string FormatString
		{
			get
			{
				return this.formatString;
			}
		}

		public SpecialFormatInfo(string name, string formatString)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			Guard.ThrowExceptionIfNullOrEmpty(formatString, "formatString");
			this.name = name;
			this.formatString = formatString;
		}

		readonly string name;

		readonly string formatString;
	}
}
