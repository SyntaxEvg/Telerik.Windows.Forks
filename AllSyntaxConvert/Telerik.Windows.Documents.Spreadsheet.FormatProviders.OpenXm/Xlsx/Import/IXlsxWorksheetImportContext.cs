using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import
{
	interface IXlsxWorksheetImportContext : IOpenXmlImportContext
	{
		IXlsxWorkbookImportContext WorkbookContext { get; }

		Worksheet Worksheet { get; }

		AutoFilterInfo AutoFilterInfo { get; set; }

		Dictionary<FloatingShapeBase, TwoCellAnchorInfo> ShapesAnchorsWaitingSize { get; }

		void ApplyColumnInfo(Range<long, ColumnInfo> columnInfo);

		void ApplyMergeCellInfo(MergedCellInfo mergedCellInfo);

		void ApplySheetProtectionInfo(WorksheetProtectionInfo sheetProtectionInfo);

		void RegisterSharedFormula(int index, SharedFormulaInfo sharedFormulaInfo);

		SharedFormulaInfo GetSharedFormula(int index);

		string GetSharedStringValue(int index);

		void ApplyCellInfo(CellInfo cellInfo);

		void ApplyRowInfo(RowInfo rowInfo);

		void ApplyDefaultColumnWidth(ColumnWidth columnWidth);

		void ApplyDefaultColumnWidthDefaultRowHeight(RowHeight rowHeight);

		void ApplyPrintOptions(PrintOptionsInfo printOptionsInfo);

		void ApplyPageMargins(PageMarginsInfo pageMarginsInfo);

		void ApplyPageSetup(PageSetupInfo pageSetupInfo);

		void ApplyHorizontalPageBreakInfo(PageBreakInfo pageBreakInfo);

		void ApplyVerticalPageBreakInfo(PageBreakInfo pageBreakInfo);

		void ApplyAutoFilter();

		void ApplySortState(SortStateInfo sortStateInfo);
	}
}
