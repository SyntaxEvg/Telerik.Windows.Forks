using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types
{
	class PdfBool : PdfSimpleType<bool>
	{
		public PdfBool()
		{
		}

		public PdfBool(bool initialValue)
			: base(initialValue)
		{
		}

		public static implicit operator PdfInt(PdfBool instance)
		{
			int defaultValue = (instance.Value ? 1 : 0);
			return new PdfInt(defaultValue);
		}

		public static implicit operator PdfReal(PdfBool instance)
		{
			int num = (instance.Value ? 1 : 0);
			return new PdfReal((double)num);
		}

		public override PdfElementType Type
		{
			get
			{
				return PdfElementType.PdfBool;
			}
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			writer.Write(base.Value.ToString().ToLower());
		}
	}
}
