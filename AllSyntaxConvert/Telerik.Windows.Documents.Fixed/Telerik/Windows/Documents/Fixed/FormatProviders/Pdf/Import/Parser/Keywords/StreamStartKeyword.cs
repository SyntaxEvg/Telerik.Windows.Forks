using System;
using System.IO;
using System.Text;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.Keywords
{
	class StreamStartKeyword : Keyword
	{
		public override string Name
		{
			get
			{
				return "stream";
			}
		}

		public override void Complete(PostScriptReader reader, IPdfImportContext context)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			reader.Reader.Seek(-1L, SeekOrigin.Current);
			reader.Reader.ReadLine();
			long position = reader.Reader.Stream.Position;
			PdfInt pdfInt = null;
			PdfDictionary pdfDictionary = (PdfDictionary)reader.PopToken();
			pdfDictionary.TryGetElement<PdfInt>(reader, context, "Length", out pdfInt);
			bool flag = false;
			if (pdfInt != null)
			{
				reader.Reader.Seek((long)pdfInt.Value, SeekOrigin.Current);
				flag = StreamStartKeyword.TryFindPdfStreamEndOnNextRow(reader.Reader);
			}
			if (!flag)
			{
				reader.Reader.Seek(position, SeekOrigin.Begin);
				int defaultValue = StreamStartKeyword.CalculateStreamLength(reader.Reader);
				pdfInt = new PdfInt(defaultValue);
			}
			reader.PushToken(new PdfStream(context, reader.Reader.Stream, pdfDictionary, pdfInt, position));
		}

		public static bool TryFindPdfStreamEndOnNextRow(Reader reader)
		{
			long position = reader.Position;
			long num = reader.IndexOf(StreamStartKeyword.EndStreamKeywordBytes);
			if (num > -1L)
			{
				using (reader.BeginReadingBlock())
				{
					reader.Seek(position, SeekOrigin.Begin);
					int count = (int)(num - position);
					byte[] array = reader.Read(count);
					foreach (byte b in array)
					{
						if (!Characters.IsCarriageReturn(b) && !Characters.IsLineFeed(b))
						{
							return false;
						}
					}
					return true;
				}
				return false;
			}
			return false;
		}

		public static int CalculateStreamLength(Reader reader)
		{
			long position = reader.Position;
			long num = StreamStartKeyword.FindPositionBeforePdfStreamEndKeyword(reader);
			return (int)(num - position + 1L);
		}

		static long FindPositionBeforePdfStreamEndKeyword(Reader reader)
		{
			long num = reader.IndexOf(StreamStartKeyword.EndStreamKeywordBytes);
			if (num > -1L)
			{
				long position = reader.Position;
				reader.Seek(num, SeekOrigin.Begin);
				byte b = reader.Peek();
				while (!Characters.IsCarriageReturn(b) && !Characters.IsLineFeed(b))
				{
					reader.Seek(-1L, SeekOrigin.Current);
					b = reader.Peek();
				}
				long result;
				if (Characters.IsLineFeed(b))
				{
					reader.Seek(-1L, SeekOrigin.Current);
					b = reader.Peek();
					result = (Characters.IsCarriageReturn(b) ? (reader.Position - 1L) : reader.Position);
				}
				else
				{
					result = reader.Position - 1L;
				}
				reader.Seek(position, SeekOrigin.Begin);
				return result;
			}
			throw new InvalidOperationException("Cannot find endstream keyword!");
		}

		internal static readonly byte[] EndStreamKeywordBytes = Encoding.UTF8.GetBytes("endstream");
	}
}
