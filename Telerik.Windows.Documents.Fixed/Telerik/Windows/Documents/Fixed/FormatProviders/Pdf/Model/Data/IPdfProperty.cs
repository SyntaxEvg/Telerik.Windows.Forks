using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data
{
	interface IPdfProperty
	{
		PdfPropertyDescriptor Descriptor { get; }

		bool HasNonDefaultValue { get; }

		bool HasValue { get; }

		void SetValue(PdfPrimitive value);

		void SetValue(PostScriptReader reader, IPdfImportContext context, PdfPrimitive value);

		PdfPrimitive GetValue();
	}
}
