using System;
using System.Globalization;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types
{
	class PdfReal : PdfSimpleType<double>
	{
		public PdfReal()
		{
		}

		public PdfReal(double initialValue)
			: base(initialValue)
		{
		}

		public override PdfElementType Type
		{
			get
			{
				return PdfElementType.PdfReal;
			}
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			string value = base.Value.ToString("f5", CultureInfo.InvariantCulture);
			writer.Write(value);
		}

		const string FormatString = "f5";
	}
}
