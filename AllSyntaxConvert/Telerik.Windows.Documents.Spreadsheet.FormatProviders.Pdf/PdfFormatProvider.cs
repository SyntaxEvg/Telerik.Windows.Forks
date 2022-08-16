using System;
using System.Collections.Generic;
using System.IO;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf
{
	public class PdfFormatProvider : global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.BinaryWorkbookFormatProviderBase
	{
		public PdfFormatProvider()
		{
			this.exporter = new global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.PdfExporter();
			this.ExportSettings = new global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.PdfExportSettings();
		}

		public override string Name
		{
			get
			{
				return global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.PdfFormatProvider.name;
			}
		}

		public override string FilesDescription
		{
			get
			{
				return global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.PdfFormatProvider.fileDescription;
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

		public override global::System.Collections.Generic.IEnumerable<string> SupportedExtensions
		{
			get
			{
				return global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.PdfFormatProvider.supportedExtensions;
			}
		}

		public global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.PdfExportSettings ExportSettings { get; set; }

		public global::Telerik.Windows.Documents.Fixed.Model.RadFixedDocument ExportToFixedDocument(global::Telerik.Windows.Documents.Spreadsheet.Model.Workbook workbook)
		{
			this.exporter.ChartRenderer = this.ExportSettings.ChartRenderer;
			global::Telerik.Windows.Documents.Fixed.Model.RadFixedDocument result;
			switch (this.ExportSettings.ExportWhat)
			{
				case global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.ExportWhat.ActiveSheet:
					result = this.exporter.CreateFixedDocument(workbook.ActiveWorksheet, this.ExportSettings.IgnorePrintArea);
					break;
				case global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.ExportWhat.EntireWorkbook:
					result = this.exporter.CreateFixedDocument(workbook, this.ExportSettings.IgnorePrintArea);
					break;
				case global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.ExportWhat.Selection:
					result = this.exporter.CreateFixedDocument(workbook.ActiveWorksheet, this.ExportSettings.SelectedRanges);
					break;
				default:
					throw new global::System.NotSupportedException("Not supported ExportWhat option!");
			}
			return result;
		}

		protected override void ExportOverride(global::Telerik.Windows.Documents.Spreadsheet.Model.Workbook workbook, global::System.IO.Stream output)
		{
			//this.exporter.PdfExportSettings =new Fixed.FormatProviders.Pdf.Export.PdfExportSettings()
			//{
			//	IsEncrypted = this.ExportSettings.PdfFileSettings.ise,
			//	OwnerPassword = FixedDocumentDefaults.Password,
			//	ImageQuality = FixedDocumentDefaults.ImageQuality,
			//	ComplianceLevel = PdfComplianceLevel.None,
			//}
	
				
				
			//	= this.ExportSettings.PdfFileSettings;



			//this.IsEncrypted = false;
			//this.OwnerPassword = FixedDocumentDefaults.Password;
			//this.ImageQuality = FixedDocumentDefaults.ImageQuality;
			//this.ComplianceLevel = PdfComplianceLevel.None;


			this.exporter.ChartRenderer = this.ExportSettings.ChartRenderer;
			switch (this.ExportSettings.ExportWhat)
			{
				case global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.ExportWhat.ActiveSheet:
					this.exporter.Export(workbook.ActiveWorksheet, this.ExportSettings.IgnorePrintArea, output);
					return;
				case global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.ExportWhat.EntireWorkbook:
					this.exporter.Export(workbook, this.ExportSettings.IgnorePrintArea, output);
					return;
				case global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.ExportWhat.Selection:
					this.exporter.Export(workbook.ActiveWorksheet, this.ExportSettings.SelectedRanges, output);
					return;
				default:
					throw new global::System.NotSupportedException("Not supported ExportWhat option!");
			}
		}

		private static readonly string name = "PdfFormatProvider";

		private static readonly string fileDescription = "PDF Documents";

		private static readonly string[] supportedExtensions = new string[] { ".pdf" };

		private readonly global::Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.PdfExporter exporter;
	}
}
