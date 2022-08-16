using System;
using System.IO;
using System.Text;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.InlineImage;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.Keywords
{
	class ImageDataKeyword : Keyword
	{
		public override string Name
		{
			get
			{
				return "ID";
			}
		}

		public override void Complete(PostScriptReader reader, IPdfImportContext context)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			bool flag = false;
			long position = reader.Reader.Stream.Position;
			PdfDictionary pdfDictionary = new PdfDictionary();
			PdfPrimitive pdfPrimitive = reader.PopToken();
			while (!(pdfPrimitive is BeginInlineImage))
			{
				PdfPrimitive pdfPrimitive2 = pdfPrimitive;
				PdfPrimitive pdfPrimitive3 = reader.PopToken();
				PdfName pdfName = (PdfName)pdfPrimitive3;
				PdfName pdfName2 = new PdfName(PdfNames.GetNameFromAbbreviation(pdfName.Value));
				if (pdfName2.Value.Equals("ColorSpace"))
				{
					PdfName pdfName3 = pdfPrimitive2 as PdfName;
					if (pdfName3 == null)
					{
						flag = true;
					}
					else
					{
						pdfPrimitive2 = new PdfName(PdfNames.GetNameFromAbbreviation(pdfName3.Value));
						pdfDictionary[pdfName2.Value] = pdfPrimitive2;
					}
				}
				else
				{
					pdfDictionary[pdfName2.Value] = pdfPrimitive2;
				}
				pdfPrimitive = reader.PopToken();
			}
			reader.Reader.Seek(position, SeekOrigin.Begin);
			int defaultValue = ImageDataKeyword.CalculateStreamLength(reader.Reader);
			PdfInt length = new PdfInt(defaultValue);
			if (!flag)
			{
				reader.PushToken(new InlineImagePdfStream(context, reader.Reader.Stream, pdfDictionary, length, position));
				reader.PushToken(new EndInlineImage());
			}
		}

		public static int CalculateStreamLength(Reader reader)
		{
			long position = reader.Position;
			long num = ImageDataKeyword.FindPositionBeforePdfStreamEndKeyword(reader);
			return (int)(num - position + 1L);
		}

		static long FindPositionBeforePdfStreamEndKeyword(Reader reader)
		{
			long num = reader.IndexOf(ImageDataKeyword.endStreamKeywordBytes);
			return num - 1L;
		}

		static readonly byte[] endStreamKeywordBytes = Encoding.UTF8.GetBytes("EI");
	}
}
