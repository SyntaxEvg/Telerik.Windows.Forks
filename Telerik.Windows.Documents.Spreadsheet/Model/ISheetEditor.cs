using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public interface ISheetEditor
	{
		Sheet Sheet { get; set; }

		event EventHandler PreviewSheetChanging;

		event EventHandler SheetChanging;

		event EventHandler PreviewSheetChanged;

		event EventHandler SheetChanged;
	}
}
