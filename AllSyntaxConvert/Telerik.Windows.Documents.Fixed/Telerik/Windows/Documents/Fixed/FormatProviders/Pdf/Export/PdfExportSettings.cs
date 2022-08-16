using System;
using Telerik.Windows.Documents.Fixed.Model;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export
{
	public class PdfExportSettings
	{
		public PdfExportSettings():base()
		{
			this.IsEncrypted = false;
			//this.OwnerPassword = "Password";
			this.OwnerPassword = FixedDocumentDefaults.Password;
			this.ImageQuality = ImageQuality.Low;
			//this.ImageQuality = FixedDocumentDefaults.ImageQuality;
			this.ComplianceLevel = PdfComplianceLevel.None;
		}

		public bool IsEncrypted { get; set; }

		public string UserPassword { get; set; }

		public ImageQuality ImageQuality { get; set; }

		public PdfComplianceLevel ComplianceLevel { get; set; }

		internal string OwnerPassword { get; set; }
	}
}
