using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class AddRemoveHyperlinkCommandContext : WorksheetCommandContextBase
	{
		public SpreadsheetHyperlink Hyperlink
		{
			get
			{
				return this.hyperlink;
			}
		}

		public AddRemoveHyperlinkCommandContext(Worksheet worksheet, SpreadsheetHyperlink hyperlink)
			: base(worksheet)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfNull<SpreadsheetHyperlink>(hyperlink, "hyperlink");
			this.hyperlink = hyperlink;
		}

		readonly SpreadsheetHyperlink hyperlink;
	}
}
