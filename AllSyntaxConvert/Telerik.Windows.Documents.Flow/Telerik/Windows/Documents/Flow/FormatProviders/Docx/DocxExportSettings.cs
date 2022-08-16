using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx
{
	public class DocxExportSettings : OpenXmlExportSettings
	{
		public DocxExportSettings()
		{
			this.AutoUpdateFields = false;
			this.InvalidDocumentAction = InvalidDocumentAction.Repair;
			this.CompatibilityVersion = new int?(15);
		}

		public bool AutoUpdateFields { get; set; }

		public InvalidDocumentAction InvalidDocumentAction { get; set; }

		internal int? CompatibilityVersion { get; set; }
	}
}
