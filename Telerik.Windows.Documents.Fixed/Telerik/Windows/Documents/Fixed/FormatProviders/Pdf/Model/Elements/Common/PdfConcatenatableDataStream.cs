using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common
{
	class PdfConcatenatableDataStream : PdfStreamObjectBase
	{
		public byte[] Data
		{
			get
			{
				if (this.data == null && this.importedStream != null)
				{
					this.data = this.importedStream.ReadDecodedPdfData();
				}
				return this.data;
			}
			protected set
			{
				this.data = value;
			}
		}

		internal static byte[] Concat(byte[] data, byte[] dataToAppend, bool concatWithWhiteSpace)
		{
			int num = ((data != null) ? data.Length : 0);
			int num2 = ((dataToAppend != null) ? dataToAppend.Length : 0);
			int num3 = num + num2;
			bool flag = concatWithWhiteSpace && PdfConcatenatableDataStream.CheckWhiteSpaceNeeded(data, dataToAppend);
			if (flag)
			{
				num3++;
			}
			byte[] array = new byte[num3];
			int num4 = 0;
			if (data != null)
			{
				data.CopyTo(array, num4);
			}
			if (dataToAppend != null)
			{
				num4 = num;
				if (flag)
				{
					array[num4] = 32;
					num4++;
				}
				dataToAppend.CopyTo(array, num4);
			}
			return array;
		}

		internal virtual void Concat(PdfConcatenatableDataStream stream)
		{
			Guard.ThrowExceptionIfNull<PdfConcatenatableDataStream>(stream, "stream");
			bool concatWithWhiteSpace = false;
			this.Data = PdfConcatenatableDataStream.Concat(this.Data, stream.Data, concatWithWhiteSpace);
		}

		protected override void InterpretData(PostScriptReader reader, IPdfImportContext context, PdfStreamBase stream)
		{
			Guard.ThrowExceptionIfNull<PdfStreamBase>(stream, "stream");
			this.importedStream = stream;
		}

		protected override byte[] GetData(IPdfExportContext context)
		{
			byte[] result = this.Data;
			this.Data = null;
			return result;
		}

		static bool CheckWhiteSpaceNeeded(byte[] data, byte[] dataToAppend)
		{
			bool flag = data != null && data.Length > 0;
			bool flag2 = dataToAppend != null && dataToAppend.Length > 0;
			if (flag && flag2)
			{
				bool flag3 = Characters.IsWhiteSpace(data[data.Length - 1]);
				bool flag4 = Characters.IsWhiteSpace(dataToAppend[0]);
				return !flag3 && !flag4;
			}
			return false;
		}

		const byte WhiteSpace = 32;

		byte[] data;

		PdfStreamBase importedStream;
	}
}
