using System;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings
{
	public class FormatStringInfo
	{
		public FormatStringType? FormatType { get; internal set; }

		public FormatStringCategory Category { get; internal set; }

		internal bool HasDateChars { get; set; }

		internal bool HasTimeChars { get; set; }

		internal FormatStringInfo(FormatStringType? formatType, FormatStringCategory category, bool hasDateChars, bool hasTimeChars)
		{
			this.FormatType = formatType;
			this.HasDateChars = hasDateChars;
			this.HasTimeChars = hasTimeChars;
			this.Category = category;
		}
	}
}
