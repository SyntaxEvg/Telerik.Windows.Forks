using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming
{
	class PdfSourceImportContext : BaseImportContext
	{
		public PdfSourceImportContext(PdfImportSettings settings)
			: base(settings)
		{
		}

		protected override void BeginImportOverride()
		{
		}
	}
}
