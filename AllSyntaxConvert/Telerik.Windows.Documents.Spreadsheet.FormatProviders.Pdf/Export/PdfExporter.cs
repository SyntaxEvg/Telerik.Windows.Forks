using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers;
using Telerik.Windows.Documents.Spreadsheet.Layout;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Printing;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export
{
	class PdfExporter
	{
		public PdfExporter()
		{
			this.pageLayout = new global::Telerik.Windows.Documents.Spreadsheet.Layout.PageLayout();
			this.layers = new global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers.PdfLayerStack();
			this.layersBuilder = new global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers.PdfLayersBuilder();
		}

		public global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers.PdfLayersBuilder LayersBuilder
		{
			get
			{
				return this.layersBuilder;
			}
		}

		public global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.PdfExportSettings PdfExportSettings { get; set; }

		public global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.IPdfChartRenderer ChartRenderer { get; set; }

		private global::Telerik.Windows.Documents.Spreadsheet.Layout.PageLayout PageLayout
		{
			get
			{
				return this.pageLayout;
			}
		}

		private global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers.PdfLayerStack Layers
		{
			get
			{
				return this.layers;
			}
		}

		public void Export(global::Telerik.Windows.Documents.Spreadsheet.Model.Workbook workbook, bool ignorePrintArea, global::System.IO.Stream output)
		{
			global::Telerik.Windows.Documents.Fixed.Model.RadFixedDocument document = this.CreateFixedDocument(workbook, ignorePrintArea);
			this.Export(document, output);
		}

		public void Export(global::Telerik.Windows.Documents.Spreadsheet.Model.Worksheet worksheet, bool ignorePrintArea, global::System.IO.Stream output)
		{
			global::Telerik.Windows.Documents.Fixed.Model.RadFixedDocument document = this.CreateFixedDocument(worksheet, ignorePrintArea);
			this.Export(document, output);
		}

		public void Export(global::Telerik.Windows.Documents.Spreadsheet.Model.Worksheet worksheet, global::System.Collections.Generic.IEnumerable<global::Telerik.Windows.Documents.Spreadsheet.Model.CellRange> selectedRanges, global::System.IO.Stream output)
		{
			global::Telerik.Windows.Documents.Fixed.Model.RadFixedDocument document = this.CreateFixedDocument(worksheet, selectedRanges);
			this.Export(document, output);
		}

		public global::Telerik.Windows.Documents.Fixed.Model.RadFixedDocument CreateFixedDocument(global::Telerik.Windows.Documents.Spreadsheet.Model.Workbook workbook, bool ignorePrintArea)
		{
			this.PageLayout.PreparePages(workbook, ignorePrintArea);
			return this.CreateFixedDocument();
		}

		public global::Telerik.Windows.Documents.Fixed.Model.RadFixedDocument CreateFixedDocument(global::Telerik.Windows.Documents.Spreadsheet.Model.Worksheet worksheet, bool ignorePrintArea)
		{
			this.PageLayout.PreparePages(worksheet, ignorePrintArea);
			return this.CreateFixedDocument();
		}

		public global::Telerik.Windows.Documents.Fixed.Model.RadFixedDocument CreateFixedDocument(global::Telerik.Windows.Documents.Spreadsheet.Model.Worksheet worksheet, global::System.Collections.Generic.IEnumerable<global::Telerik.Windows.Documents.Spreadsheet.Model.CellRange> selectedRanges)
		{
			this.PageLayout.PreparePages(worksheet, selectedRanges);
			return this.CreateFixedDocument();
		}

		private void Export(global::Telerik.Windows.Documents.Fixed.Model.RadFixedDocument document, global::System.IO.Stream output)
		{
			new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.PdfFormatProvider
			{
				ExportSettings = this.PdfExportSettings
			}.Export(document, output);
		}

		private global::Telerik.Windows.Documents.Fixed.Model.RadFixedDocument CreateFixedDocument()
		{
			this.Layers.Clear();
			this.LayersBuilder.BuildLayersOverride(this.Layers);
			global::Telerik.Windows.Documents.Fixed.Model.RadFixedDocument radFixedDocument = new global::Telerik.Windows.Documents.Fixed.Model.RadFixedDocument();
			foreach (global::Telerik.Windows.Documents.Spreadsheet.Layout.WorksheetPageLayoutBox worksheetPage in this.PageLayout.Pages)
			{
				this.RenderPage(radFixedDocument, worksheetPage);
			}
			return radFixedDocument;
		}

		private void RenderPage(global::Telerik.Windows.Documents.Fixed.Model.RadFixedDocument document, global::Telerik.Windows.Documents.Spreadsheet.Layout.WorksheetPageLayoutBox worksheetPage)
		{
			global::Telerik.Windows.Documents.Spreadsheet.Model.Printing.WorksheetPageSetup worksheetPageSetup = worksheetPage.Worksheet.WorksheetPageSetup;
			global::Telerik.Windows.Documents.Fixed.Model.RadFixedPage radFixedPage = document.Pages.AddPage();
			radFixedPage.Size = worksheetPageSetup.RotatedPageSize;
			global::Telerik.Windows.Documents.Fixed.Model.Editing.FixedContentEditor fixedContentEditor = new global::Telerik.Windows.Documents.Fixed.Model.Editing.FixedContentEditor(radFixedPage, global::Telerik.Windows.Documents.Fixed.Model.Data.MatrixPosition.Default);
			global::Telerik.Windows.Documents.Spreadsheet.Layout.RadWorksheetLayout worksheetLayout = worksheetPage.Worksheet.Workbook.GetWorksheetLayout(worksheetPage.Worksheet, true);
			global::System.Windows.Point offset = new global::System.Windows.Point(worksheetPage.Left, worksheetPage.Top);
			global::Telerik.Windows.Documents.Spreadsheet.Layout.SheetViewport sheetViewport = global::Telerik.Windows.Documents.Spreadsheet.Layout.SheetViewportFactory.CreateSheetViewport(worksheetLayout, worksheetPage.Size, offset, worksheetPage.ScaleFactor);
			global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.PdfPrintWorksheetRenderUpdateContext updateContext = new global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.PdfPrintWorksheetRenderUpdateContext(worksheetLayout, sheetViewport, worksheetPage.ScaleFactor, worksheetPage.HeaderFooterContext, this.ChartRenderer);
			double offsetX = worksheetPage.PageContentBox.Left + worksheetPage.RowColumnHeadingsSize.Width;
			double offsetY = worksheetPage.PageContentBox.Top + worksheetPage.RowColumnHeadingsSize.Height;
			fixedContentEditor.Position.Translate(offsetX, offsetY);
			this.SetShouldRender(worksheetPageSetup.PrintOptions.PrintRowColumnHeadings, "RowHeadingMargin");
			this.SetShouldRender(worksheetPageSetup.PrintOptions.PrintRowColumnHeadings, "ColumnHeadingMargin");
			this.SetShouldRender(worksheetPageSetup.PrintOptions.PrintGridlines, "Gridlines");
			this.SetShouldRender(worksheetPageSetup.PrintOptions.PrintGridlinesOutline, "GridlinesOutline");
			this.Layers.UpdateRender(updateContext, fixedContentEditor);
		}

		private void SetShouldRender(bool shouldRender, string layerName)
		{
			global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers.PdfLayerBase byName = this.Layers.GetByName(layerName);
			if (byName != null)
			{
				byName.ShouldRender = shouldRender;
			}
		}

		private readonly global::Telerik.Windows.Documents.Spreadsheet.Layout.PageLayout pageLayout;

		private readonly global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers.PdfLayerStack layers;

		private readonly global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers.PdfLayersBuilder layersBuilder;
	}
}
