using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.PrimitiveParsers
{
	class DictionaryParser
	{
		public DictionaryParser(PostScriptReader parser)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(parser, "parser");
			this.nestedDictionariesCounters = 0;
			this.parser = parser;
		}

		public void Start()
		{
			this.nestedDictionariesCounters++;
			this.parser.PushCollection();
		}

		public void End()
		{
			if (this.nestedDictionariesCounters > 0)
			{
				this.nestedDictionariesCounters--;
				Stack<PdfPrimitive> stack = this.parser.PopCollection();
				PdfDictionary pdfDictionary = new PdfDictionary();
				while (stack.Count > 0)
				{
					PdfPrimitive value = stack.Pop();
					PdfName pdfName = (PdfName)stack.Pop();
					pdfDictionary[pdfName.Value] = value;
				}
				this.parser.PushToken(pdfDictionary);
			}
		}

		int nestedDictionariesCounters;

		readonly PostScriptReader parser;
	}
}
