using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common
{
	class PrimitiveWrapper : PdfPrimitive
	{
		public PrimitiveWrapper(PdfPrimitive primitive)
		{
			Guard.ThrowExceptionIfNull<PdfPrimitive>(primitive, "primitive");
			this.primitive = primitive;
		}

		public override PdfElementType Type
		{
			get
			{
				return this.primitive.Type;
			}
		}

		internal PdfPrimitive Primitive
		{
			get
			{
				return this.primitive;
			}
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			this.primitive.Write(writer, context);
		}

		readonly PdfPrimitive primitive;
	}
}
