using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Text
{
	class TextLeading : ContentStreamOperator
	{
		public override string Name
		{
			get
			{
				return "TL";
			}
		}

		public override void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
			double value = interpreter.Operands.GetLast<PdfReal>(interpreter.Reader, context.Owner).Value;
			interpreter.TextState.TextLead = value;
		}
	}
}
