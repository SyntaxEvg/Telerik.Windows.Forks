using System;
using Telerik.Windows.Documents.Fixed.Model;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export
{
	public class PdfExportSettings
	{
		public PdfExportSettings()
		{
			this.IsEncrypted = false;
			this.OwnerPassword = FixedDocumentDefaults.Password;
			this.ImageQuality = FixedDocumentDefaults.ImageQuality;
			this.ComplianceLevel = PdfComplianceLevel.None;
		}

		public bool IsEncrypted { get; set; }

		public string UserPassword { get; set; }

		public ImageQuality ImageQuality { get; set; }

		public PdfComplianceLevel ComplianceLevel { get; set; }

		internal string OwnerPassword { get; set; }
	}
}
