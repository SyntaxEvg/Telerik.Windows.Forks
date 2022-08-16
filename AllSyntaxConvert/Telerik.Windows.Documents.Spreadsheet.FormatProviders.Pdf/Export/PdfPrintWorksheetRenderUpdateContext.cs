using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Layout;
using Telerik.Windows.Documents.Spreadsheet.Layout.Layers;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export
{
	class PdfPrintWorksheetRenderUpdateContext : PrintWorksheetRenderUpdateContext
	{
		public PdfPrintWorksheetRenderUpdateContext(RadWorksheetLayout worksheetLayout, SheetViewport sheetViewport, Size scaleFactor, HeaderFooterRenderContext headerFooterContext, IPdfChartRenderer chartRenderer)
			: base(worksheetLayout, sheetViewport, scaleFactor, headerFooterContext)
		{
			this.chartRenderer = chartRenderer;
		}

		public IPdfChartRenderer ChartRenderer
		{
			get
			{
				return this.chartRenderer;
			}
		}

		readonly IPdfChartRenderer chartRenderer;
	}
}
