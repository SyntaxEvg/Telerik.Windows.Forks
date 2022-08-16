using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Text
{
	class SetSpacingMoveToNextLineAndShowText : ContentStreamOperator
	{
		public override string Name
		{
			get
			{
				return "\"";
			}
		}

		public override void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
			PdfString last = interpreter.Operands.GetLast<PdfString>(interpreter.Reader, context.Owner);
			PdfReal last2 = interpreter.Operands.GetLast<PdfReal>(interpreter.Reader, context.Owner);
			PdfReal last3 = interpreter.Operands.GetLast<PdfReal>(interpreter.Reader, context.Owner);
			CharSpace.Execute(interpreter.TextState, last2.Value);
			WordSpace.Execute(interpreter.TextState, last3.Value);
			MoveToNextLineAndShowText.Execute(interpreter, context, last);
		}
	}
}
