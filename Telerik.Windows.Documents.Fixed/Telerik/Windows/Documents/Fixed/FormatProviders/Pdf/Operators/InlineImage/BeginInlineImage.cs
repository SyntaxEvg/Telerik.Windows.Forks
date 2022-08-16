using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.InlineImage
{
	class BeginInlineImage : ContentStreamOperator
	{
		public override string Name
		{
			get
			{
				return "BI";
			}
		}

		public override void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
		}
	}
}
