using System;
using System.Collections.Generic;
using System.IO;
using Telerik.Windows.Documents.Common.FormatProviders;
using Telerik.Windows.Documents.Flow.FormatProviders.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf
{
	public class RtfFormatProvider : TextBasedFormatProviderBase<RadFlowDocument>
	{
		public RtfFormatProvider()
		{
			this.ExportSettings = new RtfExportSettings();
		}

		public override IEnumerable<string> SupportedExtensions
		{
			get
			{
				return RtfFormatProvider.supportedExtensionsList;
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

		public RtfExportSettings ExportSettings { get; set; }

		protected override RadFlowDocument ImportOverride(Stream input)
		{
			Guard.ThrowExceptionIfNull<Stream>(input, "input");
			RtfDocumentImporter rtfDocumentImporter = new RtfDocumentImporter();
			return rtfDocumentImporter.Import(input);
		}

		protected override void ExportOverride(RadFlowDocument document, Stream output)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			Guard.ThrowExceptionIfNull<Stream>(output, "output");
			using (new RadFlowDocumentLicenseCheck(document))
			{
				RtfDocumentExporter rtfDocumentExporter = new RtfDocumentExporter(document, output, this.ExportSettings);
				rtfDocumentExporter.Export();
			}
		}

		static readonly IEnumerable<string> supportedExtensionsList = new string[] { ".rtf" };
	}
}
