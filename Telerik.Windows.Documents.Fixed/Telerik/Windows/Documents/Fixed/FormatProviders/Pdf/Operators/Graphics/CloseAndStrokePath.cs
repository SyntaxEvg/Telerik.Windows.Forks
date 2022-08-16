using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Graphics
{
	class CloseAndStrokePath : StrokePath
	{
		public override string Name
		{
			get
			{
				return "s";
			}
		}

		public override void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
			ClosePath.Execute(interpreter);
			base.Execute(interpreter, context);
		}
	}
}
