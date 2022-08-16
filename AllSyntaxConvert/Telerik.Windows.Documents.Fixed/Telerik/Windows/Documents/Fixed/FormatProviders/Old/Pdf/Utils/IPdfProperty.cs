using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils
{
	interface IPdfProperty
	{
		PdfPropertyDescriptor Descriptor { get; }

		bool SetValue(object value);
	}
}
