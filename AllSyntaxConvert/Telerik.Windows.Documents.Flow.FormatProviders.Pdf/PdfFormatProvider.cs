using System;
using System.Collections.Generic;
using System.IO;
using Telerik.Windows.Documents.Common.FormatProviders;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Flow.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Pdf
{
	public class PdfFormatProvider : BinaryFormatProviderBase<RadFlowDocument>
	{
		public PdfFormatProvider()
		{
			this.ExportSettings = new PdfExportSettings();
		}

		public PdfExportSettings ExportSettings { get; set; }

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

		public RadFixedDocument ExportToFixedDocument(RadFlowDocument document)
		{
			return new PdfExporter(new PdfExportContext(document, this.ExportSettings)).Export();
		}

		protected override void ExportOverride(global::Telerik.Windows.Documents.Flow.Model.RadFlowDocument document, global::System.IO.Stream output)
		{
			RadFixedDocument document2 = new PdfExporter(new global::Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Export.PdfExportContext(document, this.ExportSettings)).Export();
            new global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.PdfFormatProvider
            {
                ExportSettings = this.ExportSettings
            }.Export(document2, output);
        }

		static readonly IEnumerable<string> supportedExtensions = new string[] { ".pdf" };
	}
}
