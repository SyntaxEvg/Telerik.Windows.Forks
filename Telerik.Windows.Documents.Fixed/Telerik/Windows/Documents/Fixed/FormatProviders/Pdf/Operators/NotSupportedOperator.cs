using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators
{
	class NotSupportedOperator : ContentStreamOperator
	{
		public NotSupportedOperator(string name)
		{
			this.name = name;
		}

		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		public override void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
			interpreter.Operands.Clear();
		}

		readonly string name;
	}
}
