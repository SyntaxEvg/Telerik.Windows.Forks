using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Graphics
{
	class CloseFillAndStrokePathEvenOdd : FillAndStrokePathEvenOdd
	{
		public override string Name
		{
			get
			{
				return "b*";
			}
		}

		public override void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
			ClosePath.Execute(interpreter);
			base.Execute(interpreter, context);
		}
	}
}
