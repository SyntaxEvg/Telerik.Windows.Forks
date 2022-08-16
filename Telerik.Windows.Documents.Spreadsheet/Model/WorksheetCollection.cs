using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class WorksheetCollection : FilteredSheetCollection<Worksheet>
	{
		internal WorksheetCollection(SheetCollection sheetCollection)
			: base(sheetCollection)
		{
		}
	}
}
