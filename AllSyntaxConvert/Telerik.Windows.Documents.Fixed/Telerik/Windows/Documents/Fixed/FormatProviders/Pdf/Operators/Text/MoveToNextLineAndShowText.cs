using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Text
{
	class MoveToNextLineAndShowText : ContentStreamOperator
	{
		public override string Name
		{
			get
			{
				return "'";
			}
		}

		public override void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
			PdfString last = interpreter.Operands.GetLast<PdfString>(interpreter.Reader, context.Owner);
			MoveToNextLineAndShowText.Execute(interpreter, context, last);
		}

		public static void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context, PdfString text)
		{
			MoveToNextLine.Execute(interpreter.TextState);
			ShowText.Execute(interpreter, context, text);
		}
	}
}
