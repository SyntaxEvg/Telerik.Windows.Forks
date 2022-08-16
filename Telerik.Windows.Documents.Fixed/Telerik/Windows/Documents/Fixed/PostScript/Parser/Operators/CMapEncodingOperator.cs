using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators;

namespace Telerik.Windows.Documents.Fixed.PostScript.Parser.Operators
{
	abstract class CMapEncodingOperator : Operator
	{
		public abstract void Execute(CMapEncodingInterpreter interpreter, IPdfImportContext context);
	}
}
