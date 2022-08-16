using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.Model;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming
{
	public class PdfStreamWriterSettings
	{
		internal PdfStreamWriterSettings(PdfExportSettings exportSettings, RadFixedDocumentInfo documentInfo)
		{
			this.exportSettings = exportSettings;
			this.documentInfo = documentInfo;
			this.WriteAnnotations = true;
		}

		public RadFixedDocumentInfo DocumentInfo
		{
			get
			{
				return this.documentInfo;
			}
		}

		public ImageQuality ImageQuality
		{
			get
			{
				return this.exportSettings.ImageQuality;
			}
			set
			{
				this.exportSettings.ImageQuality = value;
			}
		}

		public bool WriteAnnotations { get; set; }

		readonly PdfExportSettings exportSettings;

		readonly RadFixedDocumentInfo documentInfo;
	}
}
