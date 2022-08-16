using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.State
{
	abstract class MatrixOperatorBase : ContentStreamOperator
	{
		public sealed override void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
			double value = interpreter.Operands.GetLast<PdfReal>(interpreter.Reader, context.Owner).Value;
			double value2 = interpreter.Operands.GetLast<PdfReal>(interpreter.Reader, context.Owner).Value;
			double value3 = interpreter.Operands.GetLast<PdfReal>(interpreter.Reader, context.Owner).Value;
			double value4 = interpreter.Operands.GetLast<PdfReal>(interpreter.Reader, context.Owner).Value;
			double value5 = interpreter.Operands.GetLast<PdfReal>(interpreter.Reader, context.Owner).Value;
			double value6 = interpreter.Operands.GetLast<PdfReal>(interpreter.Reader, context.Owner).Value;
			this.InterpretMatrixOverride(interpreter, new Matrix(value6, value5, value4, value3, value2, value));
		}

		protected abstract void InterpretMatrixOverride(ContentStreamInterpreter interpreter, Matrix matrix);

		public void Write(PdfWriter writer, IPdfContentExportContext context, IPosition position)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<IPosition>(position, "position");
			this.Write(writer, context, position.Matrix);
		}

		public void Write(PdfWriter writer, IPdfContentExportContext context, Matrix matrix)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			this.Write(writer, context, matrix.M11, matrix.M12, matrix.M21, matrix.M22, matrix.OffsetX, matrix.OffsetY);
		}

		void Write(PdfWriter writer, IPdfContentExportContext context, double m11, double m12, double m21, double m22, double offsetX, double offsetY)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			if (m11 != 1.0 || m12 != 0.0 || m21 != 0.0 || m22 != 1.0 || offsetX != 0.0 || offsetY != 0.0)
			{
				base.WriteInternal(writer, context.Owner, new PdfPrimitive[]
				{
					m11.ToPdfReal(),
					m12.ToPdfReal(),
					m21.ToPdfReal(),
					m22.ToPdfReal(),
					offsetX.ToPdfReal(),
					offsetY.ToPdfReal()
				});
			}
		}
	}
}
