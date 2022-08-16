using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx
{
	public class XlsxFormatProvider : BinaryWorkbookFormatProviderBase
	{
		public XlsxFormatProvider()
		{
			this.ExportSettings = new XlsxExportSettings();
			this.ImportSettings = new XlsxImportSettings();
		}

		public override string Name
		{
			get
			{
				return XlsxFormatProvider.name;
			}
		}

		public override string FilesDescription
		{
			get
			{
				return XlsxFormatProvider.fileDescription;
			}
		}

		public override IEnumerable<string> SupportedExtensions
		{
			get
			{
				return XlsxFormatProvider.supportedExtensions;
			}
		}

		public override bool CanImport
		{
			get
			{
				return true;
			}
		}

		public override bool CanExport
		{
			get
			{
				return true;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public XlsxExportSettings ExportSettings { get; set; }

		[EditorBrowsable(EditorBrowsableState.Never)]
		public XlsxImportSettings ImportSettings { get; set; }

		protected override Workbook ImportOverride(Stream input)
		{
			XlsxImporter xlsxImporter = new XlsxImporter();
			XlsxWorkbookImportContext xlsxWorkbookImportContext = new XlsxWorkbookImportContext();
			xlsxImporter.Import(input, xlsxWorkbookImportContext);
			return xlsxWorkbookImportContext.Workbook;
		}

		protected override void ExportOverride(Workbook workbook, Stream output)
		{
			XlsxExporter xlsxExporter = new XlsxExporter();
			using (new LicenseCheck(workbook))
			{
				xlsxExporter.Export(output, new XlsxWorkbookExportContext(workbook), this.ExportSettings);
			}
		}

		static readonly string name = "XlsxFormatProvider";

		static readonly string fileDescription = "Excel Workbook";

		static readonly IEnumerable<string> supportedExtensions = new string[] { ".xlsx" };
	}
}
