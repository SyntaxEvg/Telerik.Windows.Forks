using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Text
{
	class MoveToNextLine : ContentStreamOperator
	{
		public override string Name
		{
			get
			{
				return "T*";
			}
		}

		public override void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
			MoveToNextLine.Execute(interpreter.TextState);
		}

		public static void Execute(TextState textState)
		{
			TranslateText.Execute(textState, 0.0, -textState.TextLead);
		}
	}
}
