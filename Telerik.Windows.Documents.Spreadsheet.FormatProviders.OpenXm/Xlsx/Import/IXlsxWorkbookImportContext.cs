using System;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Parts;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import
{
	interface IXlsxWorkbookImportContext : IOpenXmlImportContext
	{
		ResourceIndexedTable<SharedString> SharedStrings { get; }

		StyleSheet StyleSheet { get; }

		ResourceIndexedTable<DefinedNameInfo> DefinedNames { get; }

		DifferentialFormatsImportContext DifferentialFormatsContext { get; }

		IXlsxWorksheetImportContext AddWorksheet(string worksheetName);

		IXlsxWorksheetImportContext GetWorksheetContext(string worksheetName);

		void ImportStyles();

		void SetActiveTabIndex(int activeTabIndex);

		void ApplyWorkbookProtectionInfo(WorkbookProtectionInfo workbookProtectionInfo);

		void RegisterWorksheetPart(Worksheet worksheet, WorksheetPart worksheetPart);

		WorksheetPart GetWorksheetPartFromWorksheet(Worksheet worksheet);

		IXlsxWorksheetImportContext GetWorksheetContextFromWorksheetPart(WorksheetPart worksheetPart);

		void RegisterDrawingPart(Worksheet worksheet, DrawingPart drawingPart);

		DrawingPart GetDrawingPartFromWorksheet(Worksheet worksheet);

		IXlsxWorksheetImportContext GetWorksheetContextFromDrawingPart(DrawingPart drawingPart);

		bool HasRegisteredDrawingPart(DrawingPart part);
	}
}
