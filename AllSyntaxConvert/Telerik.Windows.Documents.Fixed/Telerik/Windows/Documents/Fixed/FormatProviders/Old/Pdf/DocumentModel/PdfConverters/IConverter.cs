using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters
{
	interface IConverter
	{
		bool HandlesIndirectReference { get; }

		object Convert(Type type, PdfContentManager contentManager, object value);
	}
}
