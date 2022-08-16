using System;
using System.IO;
using System.Text;
using Telerik.Windows.Documents.Fixed.Exceptions;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import
{
	class PdfImporter
	{
		public void Import(Stream input, IPdfImportContext context)
		{
			Guard.ThrowExceptionIfNull<Stream>(input, "input");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			context.BeginImport(input);
		}

		internal static bool SeekToStartXRef(Stream input)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder("startxref");
			int num = (int)Math.Min(input.Length, 1024L);
			byte[] array = new byte[num];
			input.Seek((long)(-(long)num), SeekOrigin.End);
			long num2 = -1L;
			input.Read(array, 0, array.Length);
			for (int i = num - 1; i >= 0; i--)
			{
				stringBuilder.Insert(0, (char)array[i]);
				if (stringBuilder2.Length < stringBuilder.Length)
				{
					stringBuilder.Remove(stringBuilder.Length - 1, 1);
				}
				if (stringBuilder.Equals(stringBuilder2))
				{
					num2 = (long)(num - i);
					break;
				}
			}
			if (num2 == -1L)
			{
				return false;
			}
			input.Seek(-num2, SeekOrigin.End);
			return true;
		}

		internal static void ValidateInputStreamCanReadAndSeek(Stream input)
		{
			if (!input.CanRead || !input.CanSeek)
			{
				throw new NotSupportedStreamTypeException(input.CanRead, input.CanSeek);
			}
		}
	}
}
