using System;
using Telerik.Windows.Documents.Fixed.Model.Editing;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers
{
	interface IPdfRenderer
	{
		FixedContentEditor Editor { get; set; }
	}
}
