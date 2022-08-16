using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Parts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.DataValidation;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export
{
	interface IXlsxWorksheetExportContext : IOpenXmlExportContext
	{
		IXlsxWorkbookExportContext WorkbookContext { get; }

		int SheetNo { get; }

		string WorksheetName { get; }

		Worksheet Worksheet { get; }

		IEnumerable<SpreadsheetHyperlink> Hyperlinks { get; }

		WorksheetViewState WorksheetViewState { get; }

		IEnumerable<FloatingShapeBase> Shapes { get; }

		IEnumerable<FloatingChartShape> Charts { get; }

		IEnumerable<RowInfo> GetNonEmptyRows();

		IEnumerable<CellInfo> GetNonEmptyCellsInRow(int rowIndex);

		int GetSharedStringIndex(TextCellValue textCellValue);

		double GetDefaultColumnWidth();

		RowHeight GetDefaultRowHeight();

		IEnumerable<MergedCellInfo> GetMergedCells();

		IEnumerable<Range<long, ColumnInfo>> GetNonEmptyColumns();

		bool ContainsNonDefaultFormattingRecordIndex(WorksheetEntityBase tableEntity, long index);

		int? GetFormattingRecordIndex(WorksheetEntityBase tableEntity, long index);

		void RegisterPictureToIdRelationshipId(Image picture, string relationshipId);

		WorksheetProtectionInfo GetSheetProtectionInfo();

		void RegisterWorksheetPart(Worksheet worksheet, WorksheetPart worksheetPart);

		PrintOptionsInfo GetPrintOptionsInfo();

		PageMarginsInfo GetPageMarginsInfo();

		PageSetupInfo GetPageSetupInfo();

		PageBreaksInfo GetPageBreaksInfo();

		CellRefRange GetAutoFilterRange();

		IEnumerable<FilterColumnInfo> GetFilterColumnInfos();

		IFilterInfo GetFilterInfo(int columnId);

		FiltersInfo GetFiltersInfo(int columnId);

		CustomFiltersInfo GetCustomFilterInfo(int columnId);

		DynamicFilterInfo GetDynamicFilterInfo(int columnId);

		Top10FilterInfo GetTop10FilterInfo(int columnId);

		ColorFilterInfo GetColorFilterInfo(int columnId);

		SortStateInfo GetSortStateInfo();

		int GetDataValidationsCount();

		IEnumerable<IDataValidationRule> GetDataValidationRules();

		IEnumerable<CellRange> GetDataValidationRuleCellRanges(IDataValidationRule rule);

		IEnumerable<SortConditionInfo> GetSortConditionInfos();

		CellRange GetUsedCellRange();
	}
}
