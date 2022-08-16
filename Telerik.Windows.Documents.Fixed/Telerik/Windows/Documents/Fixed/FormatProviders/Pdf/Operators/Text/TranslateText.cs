using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Text
{
	class TranslateText : ContentStreamOperator
	{
		public override string Name
		{
			get
			{
				return "Td";
			}
		}

		public override void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
			double value = interpreter.Operands.GetLast<PdfReal>(interpreter.Reader, context.Owner).Value;
			double value2 = interpreter.Operands.GetLast<PdfReal>(interpreter.Reader, context.Owner).Value;
			TranslateText.Execute(interpreter.TextState, value2, value);
		}

		public static void Execute(TextState textState, double tx, double ty)
		{
			textState.TextLineMatrix = new Matrix(1.0, 0.0, 0.0, 1.0, tx, ty).MultiplyBy(textState.TextLineMatrix);
			textState.TextMatrix = textState.TextLineMatrix;
		}

		public static void MoveHorizontalPosition(TextState textState, double tx)
		{
			textState.TextMatrix = TranslateText.GetTranslatedTextMatrix(textState.TextMatrix, tx);
		}

		public static Matrix GetTranslatedTextMatrix(Matrix textMatrix, double tx)
		{
			return new Matrix(1.0, 0.0, 0.0, 1.0, tx, 0.0).MultiplyBy(textMatrix);
		}
	}
}
