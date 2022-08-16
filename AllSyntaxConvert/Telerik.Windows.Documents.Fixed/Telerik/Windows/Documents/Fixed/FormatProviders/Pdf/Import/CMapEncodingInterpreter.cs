using System;
using System.IO;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding;
using Telerik.Windows.Documents.Fixed.PostScript.Parser.Keywords;
using Telerik.Windows.Documents.Fixed.PostScript.Parser.Operators;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import
{
	class CMapEncodingInterpreter : InterpreterBase
	{
		public CMapEncodingInterpreter(Stream stream, IPdfImportContext context, CMapEncoding encoding)
		{
			Guard.ThrowExceptionIfNull<Stream>(stream, "stream");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<CMapEncoding>(encoding, "encoding");
			this.context = context;
			this.encoding = encoding;
			this.operands = new OperandsCollection();
			this.reader = new PostScriptReader(stream, new PostScriptKeywordCollection());
		}

		public CMapEncoding Encoding
		{
			get
			{
				return this.encoding;
			}
		}

		public OperandsCollection Operands
		{
			get
			{
				return this.operands;
			}
		}

		public PostScriptReader Reader
		{
			get
			{
				return this.reader;
			}
		}

		public void Execute()
		{
			this.Reader.Reader.Seek(0L, SeekOrigin.Begin);
			PdfPrimitive[] array = this.reader.Read(this.context);
			foreach (PdfPrimitive pdfPrimitive in array)
			{
				if (pdfPrimitive != null)
				{
					if (pdfPrimitive.Type == PdfElementType.Operator)
					{
						CMapEncodingOperator cmapEncodingOperator = (CMapEncodingOperator)pdfPrimitive;
						cmapEncodingOperator.Execute(this, this.context);
					}
					else
					{
						this.Operands.AddLast(pdfPrimitive);
					}
				}
			}
		}

		readonly CMapEncoding encoding;

		readonly OperandsCollection operands;

		readonly PostScriptReader reader;

		readonly IPdfImportContext context;
	}
}
