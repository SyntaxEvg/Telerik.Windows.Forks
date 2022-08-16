using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data
{
	enum PdfPropertyRestrictions
	{
		None,
		MustBeIndirectReference,
		MustBeDirectObject,
		ContainsOnlyIndirectReferences
	}
}
