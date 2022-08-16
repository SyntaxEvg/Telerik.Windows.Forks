using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;

namespace Telerik.Windows.Documents.Fixed.PostScript.Parser.Operators
{
	class BeginBFChar : CMapEncodingOperator
	{
		public override string Name
		{
			get
			{
				return "beginbfchar";
			}
		}

		public override void Execute(CMapEncodingInterpreter interpreter, IPdfImportContext context)
		{
			interpreter.Operands.Clear();
		}
	}
}
