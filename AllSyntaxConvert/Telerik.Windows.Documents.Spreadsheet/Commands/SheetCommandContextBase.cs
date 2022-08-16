using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Commands
{
	abstract class SheetCommandContextBase : WorkbookCommandContextBase
	{
		public Sheet Sheet
		{
			get
			{
				return this.sheet;
			}
		}

		public SheetCommandContextBase(Sheet sheet)
			: base(sheet.Workbook)
		{
			this.sheet = sheet;
		}

		internal override void InvalidateLayout()
		{
			this.Sheet.InvalidateLayout();
		}

		readonly Sheet sheet;
	}
}
