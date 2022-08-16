using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Printing;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class AddRemovePageBreakCommandContext : WorksheetCommandContextBase
	{
		public PageBreak PageBreak
		{
			get
			{
				return this.pageBreak;
			}
		}

		public AddRemovePageBreakCommandContext(Worksheet worksheet, PageBreak pageBreak)
			: base(worksheet)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfNull<PageBreak>(pageBreak, "pageBreak");
			this.pageBreak = pageBreak;
		}

		readonly PageBreak pageBreak;
	}
}
