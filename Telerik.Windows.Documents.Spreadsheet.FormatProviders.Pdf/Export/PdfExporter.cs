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
			this.pageLayout = new PageLayout();
			this.layers = new PdfLayerStack();
			this.layersBuilder = new PdfLayersBuilder();
		}

		public PdfLayersBuilder LayersBuilder
		{
			get
			{
				return this.layersBuilder;
			}
		}

		public PdfExportSettings PdfExportSettings { get; set; }

		public IPdfChartRenderer ChartRenderer { get; set; }

		PageLayout PageLayout
		{
			get
			{
				return this.pageLayout;
			}
		}

		PdfLayerStack Layers
		{
			get
			{
				return this.layers;
			}
		}

		public void Export(Workbook workbook, bool ignorePrintArea, Stream output)
		{
			RadFixedDocument document = this.CreateFixedDocument(workbook, ignorePrintArea);
			this.Export(document, output);
		}

		public void Export(Worksheet worksheet, bool ignorePrintArea, Stream output)
		{
			RadFixedDocument document = this.CreateFixedDocument(worksheet, ignorePrintArea);
			this.Export(document, output);
		}

		public void Export(Worksheet worksheet, IEnumerable<CellRange> selectedRanges, Stream output)
		{
			RadFixedDocument document = this.CreateFixedDocument(worksheet, selectedRanges);
			this.Export(document, output);
		}

		public RadFixedDocument CreateFixedDocument(Workbook workbook, bool ignorePrintArea)
		{
			this.PageLayout.PreparePages(workbook, ignorePrintArea);
			return this.CreateFixedDocument();
		}

		public RadFixedDocument CreateFixedDocument(Worksheet worksheet, bool ignorePrintArea)
		{
			this.PageLayout.PreparePages(worksheet, ignorePrintArea);
			return this.CreateFixedDocument();
		}

		public RadFixedDocument CreateFixedDocument(Worksheet worksheet, IEnumerable<CellRange> selectedRanges)
		{
			this.PageLayout.PreparePages(worksheet, selectedRanges);
			return this.CreateFixedDocument();
		}

		void Export(RadFixedDocument document, Stream output)
		{
			new PdfFormatProvider
			{
				ExportSettings = this.PdfExportSettings
			}.Export(document, output);
		}

		RadFixedDocument CreateFixedDocument()
		{
			this.Layers.Clear();
			this.LayersBuilder.BuildLayersOverride(this.Layers);
			RadFixedDocument radFixedDocument = new RadFixedDocument();
			foreach (WorksheetPageLayoutBox worksheetPage in this.PageLayout.Pages)
			{
				this.RenderPage(radFixedDocument, worksheetPage);
			}
			return radFixedDocument;
		}

		void RenderPage(RadFixedDocument document, WorksheetPageLayoutBox worksheetPage)
		{
			WorksheetPageSetup worksheetPageSetup = worksheetPage.Worksheet.WorksheetPageSetup;
			RadFixedPage radFixedPage = document.Pages.AddPage();
			radFixedPage.Size = worksheetPageSetup.RotatedPageSize;
			FixedContentEditor fixedContentEditor = new FixedContentEditor(radFixedPage, MatrixPosition.Default);
			RadWorksheetLayout worksheetLayout = worksheetPage.Worksheet.Workbook.GetWorksheetLayout(worksheetPage.Worksheet, true);
			Point offset = new Point(worksheetPage.Left, worksheetPage.Top);
			SheetViewport sheetViewport = SheetViewportFactory.CreateSheetViewport(worksheetLayout, worksheetPage.Size, offset, worksheetPage.ScaleFactor);
			PdfPrintWorksheetRenderUpdateContext updateContext = new PdfPrintWorksheetRenderUpdateContext(worksheetLayout, sheetViewport, worksheetPage.ScaleFactor, worksheetPage.HeaderFooterContext, this.ChartRenderer);
			double offsetX = worksheetPage.PageContentBox.Left + worksheetPage.RowColumnHeadingsSize.Width;
			double offsetY = worksheetPage.PageContentBox.Top + worksheetPage.RowColumnHeadingsSize.Height;
			fixedContentEditor.Position.Translate(offsetX, offsetY);
			this.SetShouldRender(worksheetPageSetup.PrintOptions.PrintRowColumnHeadings, "RowHeadingMargin");
			this.SetShouldRender(worksheetPageSetup.PrintOptions.PrintRowColumnHeadings, "ColumnHeadingMargin");
			this.SetShouldRender(worksheetPageSetup.PrintOptions.PrintGridlines, "Gridlines");
			this.SetShouldRender(worksheetPageSetup.PrintOptions.PrintGridlinesOutline, "GridlinesOutline");
			this.Layers.UpdateRender(updateContext, fixedContentEditor);
		}

		void SetShouldRender(bool shouldRender, string layerName)
		{
			PdfLayerBase byName = this.Layers.GetByName(layerName);
			if (byName != null)
			{
				byName.ShouldRender = shouldRender;
			}
		}

		readonly PageLayout pageLayout;

		readonly PdfLayerStack layers;

		readonly PdfLayersBuilder layersBuilder;
	}
}
