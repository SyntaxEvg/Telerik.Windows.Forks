using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators
{
	abstract class ContentStreamOperator : Operator
	{
		public abstract void Execute(ContentStreamInterpreter contentStreamInterpreter, IPdfContentImportContext context);
	}
}
