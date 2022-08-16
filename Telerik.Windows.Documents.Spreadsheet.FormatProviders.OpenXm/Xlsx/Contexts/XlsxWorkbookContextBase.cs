using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Common.Model.Data;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Parts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Theming;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	abstract class XlsxWorkbookContextBase<TWorksheetContext> where TWorksheetContext : XlsxWorksheetContextBase
	{
		public XlsxWorkbookContextBase(bool isExport, Workbook workbook)
		{
			Guard.ThrowExceptionIfNull<Workbook>(workbook, "workbook");
			this.workbook = workbook;
			this.worksheetToContext = new Dictionary<Worksheet, TWorksheetContext>();
			this.worksheetToWorksheetPartMapping = new ValueMapper<Worksheet, WorksheetPart>();
			this.worksheetToDrawingPartMapping = new ValueMapper<Worksheet, DrawingPart>();
			this.sharedStrings = new ResourceIndexedTable<SharedString>(isExport, 0);
			this.styleSheet = new StyleSheet(isExport);
			this.definedNames = new ResourceIndexedTable<DefinedNameInfo>(true, 0);
			this.chartToChartPartMapping = new ValueMapper<ChartShape, ChartPart>();
		}

		public Workbook Workbook
		{
			get
			{
				return this.workbook;
			}
		}

		public ResourceManager Resources
		{
			get
			{
				return this.Workbook.Resources;
			}
		}

		public ResourceIndexedTable<SharedString> SharedStrings
		{
			get
			{
				return this.sharedStrings;
			}
		}

		public StyleSheet StyleSheet
		{
			get
			{
				return this.styleSheet;
			}
		}

		public DocumentTheme Theme
		{
			get
			{
				return this.workbook.Theme;
			}
			set
			{
				this.workbook.Theme = value;
			}
		}

		public ResourceIndexedTable<DefinedNameInfo> DefinedNames
		{
			get
			{
				return this.definedNames;
			}
		}

		public TWorksheetContext GetWorksheetContext(Worksheet worksheet)
		{
			return this.worksheetToContext[worksheet];
		}

		public TWorksheetContext GetWorksheetContext(string worksheetName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(worksheetName, "worksheetName");
			Worksheet byName = this.Workbook.Worksheets.GetByName(worksheetName);
			TWorksheetContext result = default(TWorksheetContext);
			if (byName != null)
			{
				result = this.worksheetToContext[byName];
			}
			return result;
		}

		public IEnumerable<TWorksheetContext> GetWorksheetContexts()
		{
			return this.worksheetToContext.Values;
		}

		public void AddDefinedNameInfo(DefinedNameInfo definedNameInfo)
		{
			this.definedNames.Add(definedNameInfo);
		}

		public void RegisterWorksheetPart(Worksheet worksheet, WorksheetPart worksheetPart)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfNull<WorksheetPart>(worksheetPart, "worksheetPart");
			this.worksheetToWorksheetPartMapping.AddPair(worksheet, worksheetPart);
		}

		public WorksheetPart GetWorksheetPartFromWorksheet(Worksheet worksheet)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			return this.worksheetToWorksheetPartMapping.GetToValue(worksheet);
		}

		public Worksheet GetWorksheetFromWorksheetPart(WorksheetPart worksheetPart)
		{
			Guard.ThrowExceptionIfNull<WorksheetPart>(worksheetPart, "worksheetPart");
			return this.worksheetToWorksheetPartMapping.GetFromValue(worksheetPart);
		}

		public void RegisterDrawingPart(Worksheet worksheet, DrawingPart drawingPart)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfNull<DrawingPart>(drawingPart, "drawingPart");
			this.worksheetToDrawingPartMapping.AddPair(worksheet, drawingPart);
		}

		public bool HasRegisteredDrawingPart(DrawingPart drawingPart)
		{
			Guard.ThrowExceptionIfNull<DrawingPart>(drawingPart, "drawingPart");
			return this.worksheetToDrawingPartMapping.ContainsToValue(drawingPart);
		}

		public DrawingPart GetDrawingPartFromWorksheet(Worksheet worksheet)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			return this.worksheetToDrawingPartMapping.GetToValue(worksheet);
		}

		public Worksheet GetWorksheetFromDrawingPart(DrawingPart drawingPart)
		{
			Guard.ThrowExceptionIfNull<DrawingPart>(drawingPart, "drawingPart");
			return this.worksheetToDrawingPartMapping.GetFromValue(drawingPart);
		}

		public void RegisterChartPartForChart(ChartShape chart, ChartPart chartPart)
		{
			Guard.ThrowExceptionIfNull<ChartShape>(chart, "chart");
			Guard.ThrowExceptionIfNull<ChartPart>(chartPart, "chartPart");
			this.chartToChartPartMapping.AddPair(chart, chartPart);
		}

		public ChartPart GetChartPartForChart(ChartShape chart)
		{
			Guard.ThrowExceptionIfNull<ChartShape>(chart, "chart");
			return this.chartToChartPartMapping.GetToValue(chart);
		}

		public ChartShape GetChartForChartPart(ChartPart chartPart)
		{
			Guard.ThrowExceptionIfNull<ChartPart>(chartPart, "chartPart");
			ChartShape result;
			this.chartToChartPartMapping.TryGetFromValue(chartPart, out result);
			return result;
		}

		protected void InitDefaultNumberFormatting()
		{
			foreach (KeyValuePair<string, int> keyValuePair in NumberFormatTypes.BuiltInFormatStrings)
			{
				this.StyleSheet.CellValueFormatTable.Add(new CellValueFormat(keyValuePair.Key), keyValuePair.Value);
			}
		}

		protected void AddWorksheetContext(TWorksheetContext context)
		{
			Guard.ThrowExceptionIfNull<TWorksheetContext>(context, "context");
			this.worksheetToContext.Add(context.Worksheet, context);
		}

		readonly Workbook workbook;

		readonly Dictionary<Worksheet, TWorksheetContext> worksheetToContext;

		readonly ResourceIndexedTable<SharedString> sharedStrings;

		readonly StyleSheet styleSheet;

		readonly ResourceIndexedTable<DefinedNameInfo> definedNames;

		readonly ValueMapper<Worksheet, WorksheetPart> worksheetToWorksheetPartMapping;

		readonly ValueMapper<Worksheet, DrawingPart> worksheetToDrawingPartMapping;

		readonly ValueMapper<ChartShape, ChartPart> chartToChartPartMapping;
	}
}
