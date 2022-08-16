using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.PostScript.Parser.Operators
{
	class EndBFChar : CMapEncodingOperator
	{
		public override string Name
		{
			get
			{
				return "endbfchar";
			}
		}

		public override void Execute(CMapEncodingInterpreter interpreter, IPdfImportContext context)
		{
			Guard.ThrowExceptionIfNull<CMapEncodingInterpreter>(interpreter, "interpreter");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			for (int i = 0; i < interpreter.Operands.Count; i += 2)
			{
				PdfString pdfString = (PdfString)interpreter.Operands.GetElementAt(Origin.Begin, i);
				PdfString pdfString2 = (PdfString)interpreter.Operands.GetElementAt(Origin.Begin, i + 1);
				string unicode = PdfString.GetUtfEncodedTextString(pdfString2.Value).ToString();
				interpreter.Encoding.AddUnicodeMapping(new CharCode(pdfString.Value), unicode);
			}
		}
	}
}
