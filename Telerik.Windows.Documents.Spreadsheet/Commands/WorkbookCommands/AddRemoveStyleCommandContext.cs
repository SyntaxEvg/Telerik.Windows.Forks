using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorkbookCommands
{
	class AddRemoveStyleCommandContext : WorkbookCommandContextBase
	{
		public CellStyle CellStyle
		{
			get
			{
				return this.cellStyle;
			}
		}

		public AddRemoveStyleCommandContext(Workbook workbook, CellStyle cellStyle)
			: base(workbook)
		{
			Guard.ThrowExceptionIfNull<CellStyle>(cellStyle, "cellStyle");
			this.cellStyle = cellStyle;
		}

		readonly CellStyle cellStyle;
	}
}
