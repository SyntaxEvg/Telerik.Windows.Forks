using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Text
{
	class TranslateTextAndSetLeading : ContentStreamOperator
	{
		public override string Name
		{
			get
			{
				return "TD";
			}
		}

		public override void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
			double value = interpreter.Operands.GetLast<PdfReal>(interpreter.Reader, context.Owner).Value;
			double value2 = interpreter.Operands.GetLast<PdfReal>(interpreter.Reader, context.Owner).Value;
			interpreter.TextState.TextLead = -value;
			TranslateText.Execute(interpreter.TextState, value2, value);
		}
	}
}
