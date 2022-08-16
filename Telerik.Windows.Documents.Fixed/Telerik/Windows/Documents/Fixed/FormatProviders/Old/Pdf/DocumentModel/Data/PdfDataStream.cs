using System;
using System.IO;
using Telerik.Windows.Documents.Core.PostScript.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Filters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader.Readers;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data
{
	class PdfDataStream
	{
		public PdfDataStream(PdfReader reader, PdfDictionaryOld dictionary)
			: this(reader, dictionary, null)
		{
		}

		public PdfDataStream(PdfReader reader, PdfDictionaryOld dictionary, IndirectObjectOld containingObject)
		{
			Guard.ThrowExceptionIfNull<PdfReader>(reader, "reader");
			this.reader = reader;
			this.dictionary = dictionary;
			this.containingObject = containingObject;
		}

		public PdfDictionaryOld Dictionary
		{
			get
			{
				return this.dictionary;
			}
		}

		public bool HasFilter
		{
			get
			{
				return this.Dictionary != null && this.Dictionary.ContainsKey("Filter");
			}
		}

		public void FixLengthAndPosition(long position)
		{
			lock (this.reader)
			{
				this.reader.Seek(position, SeekOrigin.Begin);
				int num;
				this.Dictionary.TryGetInt("Length", out num);
				this.reader.Seek(position, SeekOrigin.Begin);
				this.reader.GoToNextLine(true);
				this.position = this.reader.Position;
				this.length = this.ReadRawData(this.position, num).Length;
				this.reader.ReadToken();
			}
		}

		public byte[] ReadData(PdfContentManager contentManager)
		{
			byte[] data = this.ReadDataWithoutDecoding(contentManager);
			return this.DecodeData(contentManager, data);
		}

		public byte[] ReadDataWithoutDecoding(PdfContentManager contentManager)
		{
			byte[] array = this.ReadRawData(this.position, this.length);
			if (this.containingObject != null)
			{
				array = PdfDataStream.DecryptData(array, this.containingObject, contentManager);
			}
			return array;
		}

		static byte[] DecryptData(byte[] data, IndirectObjectOld containingObject, PdfContentManager contentManager)
		{
			return contentManager.DecryptStream(containingObject, data);
		}

		byte[] DecodeData(PdfContentManager contentManager, byte[] data)
		{
			if (!this.HasFilter)
			{
				return data;
			}
			PdfNameOld[] filters = FiltersManagerOld.GetFilters(contentManager, this.Dictionary);
			DecodeParameters[] decodeParameters = FiltersManagerOld.GetDecodeParameters(contentManager, this.Dictionary);
			return FiltersManagerOld.Decode(this.Dictionary, filters, data, decodeParameters);
		}

		byte[] ReadRawData(long offset, int length)
		{
			StreamPart streamPart = null;
			byte[] result;
			try
			{
				this.reader.Seek(offset + (long)length, SeekOrigin.Begin);
				if (this.reader.ReadToken() == Token.StreamEnd)
				{
					streamPart = new StreamPart(this.reader.Stream, offset, length);
				}
				else
				{
					streamPart = new StreamPart(this.reader.Stream, offset, "endstream");
				}
				result = this.reader.ReadRawData(offset, streamPart);
			}
			finally
			{
				if (streamPart != null)
				{
					streamPart.Dispose();
				}
			}
			return result;
		}

		readonly IndirectObjectOld containingObject;

		readonly PdfDictionaryOld dictionary;

		readonly PdfReader reader;

		long position;

		int length;
	}
}
