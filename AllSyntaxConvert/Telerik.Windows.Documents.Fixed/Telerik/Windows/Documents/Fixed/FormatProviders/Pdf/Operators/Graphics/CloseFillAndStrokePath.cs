using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Graphics
{
	class CloseFillAndStrokePath : FillAndStrokePath
	{
		public override string Name
		{
			get
			{
				return "b";
			}
		}

		public override void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
			ClosePath.Execute(interpreter);
			base.Execute(interpreter, context);
		}
	}
}
