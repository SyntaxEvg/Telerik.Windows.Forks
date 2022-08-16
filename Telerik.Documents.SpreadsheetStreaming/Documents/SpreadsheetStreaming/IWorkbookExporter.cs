using System;
using System.Collections.Generic;

namespace Telerik.Documents.SpreadsheetStreaming
{
	public interface IWorkbookExporter : IDisposable
	{
		SpreadCellStyleCollection CellStyles { get; }

		IWorksheetExporter CreateWorksheetExporter(string name);

		IEnumerable<SheetInfo> GetSheetInfos();
	}
}
