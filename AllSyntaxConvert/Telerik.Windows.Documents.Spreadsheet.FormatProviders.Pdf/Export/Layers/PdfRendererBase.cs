using System;
using Telerik.Windows.Documents.Fixed.Model.Editing;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers
{
	abstract class PdfRendererBase : IPdfRenderer
	{
		public FixedContentEditor Editor { get; set; }
	}
}
