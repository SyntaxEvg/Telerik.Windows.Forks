using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types
{
	class PdfInt : PdfSimpleType<int>
	{
		public PdfInt()
		{
		}

		public PdfInt(int defaultValue)
			: base(defaultValue)
		{
		}

		public static implicit operator PdfReal(PdfInt instance)
		{
			return new PdfReal((double)instance.Value);
		}

		public override PdfElementType Type
		{
			get
			{
				return PdfElementType.PdfInt;
			}
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			writer.Write(base.Value.ToString());
		}
	}
}
