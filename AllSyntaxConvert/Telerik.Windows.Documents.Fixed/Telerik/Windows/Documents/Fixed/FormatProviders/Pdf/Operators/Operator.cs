using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators
{
	abstract class Operator : PdfPrimitive
	{
		public override PdfElementType Type
		{
			get
			{
				return PdfElementType.Operator;
			}
		}

		public abstract string Name { get; }

		public sealed override void Write(PdfWriter writer, IPdfExportContext context)
		{
			writer.WriteLine(this.Name);
		}

		protected void WriteInternal(PdfWriter writer, IPdfExportContext context, params PdfPrimitive[] operands)
		{
			foreach (PdfPrimitive pdfPrimitive in operands)
			{
				pdfPrimitive.Write(writer, context);
				writer.WriteSeparator();
			}
			this.Write(writer, context);
		}
	}
}
