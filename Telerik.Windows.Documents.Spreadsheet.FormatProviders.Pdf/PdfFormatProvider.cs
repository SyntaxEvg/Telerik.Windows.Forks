using System;
using System.Collections.Generic;
using System.IO;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf
{
	public class PdfFormatProvider : BinaryWorkbookFormatProviderBase
	{
		public PdfFormatProvider()
		{
			this.exporter = new PdfExporter();
			this.ExportSettings = new PdfExportSettings();
		}

		public override string Name
		{
			get
			{
				return PdfFormatProvider.name;
			}
		}

		public override string FilesDescription
		{
			get
			{
				return PdfFormatProvider.fileDescription;
			}
		}

		public override bool CanExport
		{
			get
			{
				return true;
			}
		}

		public override bool CanImport
		{
			get
			{
				return false;
			}
		}

		public override IEnumerable<string> SupportedExtensions
		{
			get
			{
				return PdfFormatProvider.supportedExtensions;
			}
		}

		public PdfExportSettings ExportSettings { get; set; }

		public RadFixedDocument ExportToFixedDocument(Workbook workbook)
		{
			this.exporter.ChartRenderer = this.ExportSettings.ChartRenderer;
			RadFixedDocument result;
			switch (this.ExportSettings.ExportWhat)
			{
			case ExportWhat.ActiveSheet:
				result = this.exporter.CreateFixedDocument(workbook.ActiveWorksheet, this.ExportSettings.IgnorePrintArea);
				break;
			case ExportWhat.EntireWorkbook:
				result = this.exporter.CreateFixedDocument(workbook, this.ExportSettings.IgnorePrintArea);
				break;
			case ExportWhat.Selection:
				result = this.exporter.CreateFixedDocument(workbook.ActiveWorksheet, this.ExportSettings.SelectedRanges);
				break;
			default:
				throw new NotSupportedException("Not supported ExportWhat option!");
			}
			return result;
		}

		protected override void ExportOverride(Workbook workbook, Stream output)
		{
			this.exporter.PdfExportSettings = this.ExportSettings.PdfFileSettings;
			this.exporter.ChartRenderer = this.ExportSettings.ChartRenderer;
			switch (this.ExportSettings.ExportWhat)
			{
			case ExportWhat.ActiveSheet:
				this.exporter.Export(workbook.ActiveWorksheet, this.ExportSettings.IgnorePrintArea, output);
				return;
			case ExportWhat.EntireWorkbook:
				this.exporter.Export(workbook, this.ExportSettings.IgnorePrintArea, output);
				return;
			case ExportWhat.Selection:
				this.exporter.Export(workbook.ActiveWorksheet, this.ExportSettings.SelectedRanges, output);
				return;
			default:
				throw new NotSupportedException("Not supported ExportWhat option!");
			}
		}

		static readonly string name = "PdfFormatProvider";

		static readonly string fileDescription = "PDF Documents";

		static readonly string[] supportedExtensions = new string[] { ".pdf" };

		readonly PdfExporter exporter;
	}
}
