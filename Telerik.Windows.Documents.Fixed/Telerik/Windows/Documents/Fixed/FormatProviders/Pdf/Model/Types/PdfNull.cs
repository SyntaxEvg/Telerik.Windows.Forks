using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types
{
	class PdfNull : PdfPrimitive
	{
		public static PdfNull Instance
		{
			get
			{
				return PdfNull.instance;
			}
		}

		public override PdfElementType Type
		{
			get
			{
				return PdfElementType.Null;
			}
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			writer.Write("null");
		}

		public const string NullValue = "null";

		static readonly PdfNull instance = new PdfNull();
	}
}
