using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public interface ISheetViewState
	{
		ThemableColor TabColor { get; set; }

		bool IsInvalidated { get; set; }
	}
}
