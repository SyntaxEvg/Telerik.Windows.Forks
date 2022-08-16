using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Parts;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export
{
	interface IXlsxWorkbookExportContext : IOpenXmlExportContext
	{
		ResourceIndexedTable<SharedString> SharedStrings { get; }

		StyleSheet StyleSheet { get; }

		IEnumerable<IXlsxWorksheetExportContext> WorksheetContexts { get; }

		ResourceIndexedTable<DefinedNameInfo> DefinedNames { get; }

		DifferentialFormatsExportContext DifferentialFormatsContext { get; }

		void RegisterWorksheetContext(string relationshipId, IXlsxWorksheetExportContext context);

		IXlsxWorksheetExportContext GetWorksheetContextByRelationshipId(string worksheetName);

		IEnumerable<WorksheetEntityBase> GetWorksheetEntitiesFromWorksheet(Worksheet worksheet);

		string GetRelationshipIdFromWorksheetContext(IXlsxWorksheetExportContext context);

		int GetActiveTabIndex();

		WorkbookProtectionInfo GetWorkbookProtectionInfo();

		void RegisterWorksheetPart(Worksheet worksheet, WorksheetPart worksheetPart);

		WorksheetPart GetWorksheetPartFromWorksheet(Worksheet worksheet);

		IXlsxWorksheetExportContext GetWorksheetContextFromWorksheetPart(WorksheetPart worksheetPart);

		void RegisterDrawingPart(Worksheet worksheet, DrawingPart drawingPart);

		DrawingPart GetDrawingPartFromWorksheet(Worksheet worksheet);

		void RegisterChartPartForChart(ChartShape chart, ChartPart chartPart);

		IXlsxWorksheetExportContext GetWorksheetContextFromDrawingPart(DrawingPart drawingPart);
	}
}
