using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils
{
	enum PdfPropertyState
	{
		None,
		MustBeIndirectReference,
		MustBeDirectObject,
		ContainsOnlyIndirectReferences
	}
}
