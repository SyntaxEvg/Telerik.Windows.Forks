using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public interface ISheet
	{
		string Name { get; set; }

		Workbook Workbook { get; }

		SheetVisibility Visibility { get; }

		SheetType Type { get; }

		ISheetViewState ViewState { get; }

		bool IsLayoutUpdateSuspended { get; }

		void InvalidateLayout();

		void SuspendLayoutUpdate();

		void ResumeLayoutUpdate();

		event EventHandler LayoutInvalidated;
	}
}
