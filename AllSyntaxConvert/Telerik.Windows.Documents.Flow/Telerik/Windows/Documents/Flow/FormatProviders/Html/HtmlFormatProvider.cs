using System;
using System.Collections.Generic;
using System.IO;
using Telerik.Windows.Documents.Common.FormatProviders;
using Telerik.Windows.Documents.Flow.FormatProviders.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html
{
	public class HtmlFormatProvider : TextBasedFormatProviderBase<RadFlowDocument>
	{
		public HtmlFormatProvider()
		{
			this.ImportSettings = new HtmlImportSettings();
			this.ExportSettings = new HtmlExportSettings();
		}

		public override IEnumerable<string> SupportedExtensions
		{
			get
			{
				return HtmlFormatProvider.supportedExtensions;
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

		public HtmlImportSettings ImportSettings { get; set; }

		public HtmlExportSettings ExportSettings { get; set; }

		protected override void ExportOverride(RadFlowDocument document, Stream output)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			Guard.ThrowExceptionIfNull<Stream>(output, "output");
			using (HtmlWriter htmlWriter = new HtmlWriter(output, this.ExportSettings))
			{
				using (new RadFlowDocumentLicenseCheck(document))
				{
					HtmlExporter htmlExporter = new HtmlExporter();
					HtmlExportContext context = new HtmlExportContext(document, this.ExportSettings);
					htmlExporter.Export(htmlWriter, context);
				}
			}
		}

		protected override RadFlowDocument ImportOverride(Stream input)
		{
			Guard.ThrowExceptionIfNull<Stream>(input, "input");
			HtmlImporter htmlImporter = new HtmlImporter();
			return htmlImporter.Import(input, new HtmlImportContext(this.ImportSettings));
		}

		static readonly IEnumerable<string> supportedExtensions = new string[] { ".html", ".htm" };
	}
}
