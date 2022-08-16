using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Utils;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Pdf.Export
{
	public class PdfExportSettings : Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.PdfExportSettings
	{
		public PdfExportSettings()
		{
			this.extensibilityManager = new ExtensibilityManager();
		}

		public ExtensibilityManager ExtensibilityManager
		{
			get
			{
				return this.extensibilityManager;
			}
		}

		readonly ExtensibilityManager extensibilityManager;
	}
}
