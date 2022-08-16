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
		public PdfDataStream(global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader.Readers.PdfReader reader, global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data.PdfDictionaryOld dictionary)
			: this(reader, dictionary, null)
		{
		}

		public PdfDataStream(global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader.Readers.PdfReader reader, global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data.PdfDictionaryOld dictionary, global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data.IndirectObjectOld containingObject)
		{
			global::Telerik.Windows.Documents.Utilities.Guard.ThrowExceptionIfNull<global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader.Readers.PdfReader>(reader, "reader");
			this.reader = reader;
			this.dictionary = dictionary;
			this.containingObject = containingObject;
		}

		public global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data.PdfDictionaryOld Dictionary
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
				this.reader.Seek(position, global::System.IO.SeekOrigin.Begin);
				int num;
				this.Dictionary.TryGetInt("Length", out num);
				this.reader.Seek(position, global::System.IO.SeekOrigin.Begin);
				this.reader.GoToNextLine(true);
				this.position = this.reader.Position;
				this.length = this.ReadRawData(this.position, num).Length;
				this.reader.ReadToken();
			}
		}

		public byte[] ReadData(global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader.PdfContentManager contentManager)
		{
			byte[] data = this.ReadDataWithoutDecoding(contentManager);
			return this.DecodeData(contentManager, data);
		}

		public byte[] ReadDataWithoutDecoding(global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader.PdfContentManager contentManager)
		{
			byte[] array = this.ReadRawData(this.position, this.length);
			if (this.containingObject != null)
			{
				array = global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data.PdfDataStream.DecryptData(array, this.containingObject, contentManager);
			}
			return array;
		}

		private static byte[] DecryptData(byte[] data, global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data.IndirectObjectOld containingObject, global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader.PdfContentManager contentManager)
		{
			return contentManager.DecryptStream(containingObject, data);
		}

		private byte[] DecodeData(global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader.PdfContentManager contentManager, byte[] data)
		{
			if (!this.HasFilter)
			{
				return data;
			}
			global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data.PdfNameOld[] filters = global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Filters.FiltersManagerOld.GetFilters(contentManager, this.Dictionary);
			global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters.DecodeParameters[] decodeParameters = global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Filters.FiltersManagerOld.GetDecodeParameters(contentManager, this.Dictionary);
			return global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Filters.FiltersManagerOld.Decode(this.Dictionary, filters, data, decodeParameters);
		}

		private byte[] ReadRawData(long offset, int length)
		{
			global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data.StreamPart streamPart = null;
			byte[] result;
			try
			{
				this.reader.Seek(offset + (long)length, global::System.IO.SeekOrigin.Begin);
				if (this.reader.ReadToken() == global::Telerik.Windows.Documents.Core.PostScript.Data.Token.StreamEnd)
				{
					streamPart = new global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data.StreamPart(this.reader.Stream, offset, length);
				}
				else
				{
					streamPart = new global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data.StreamPart(this.reader.Stream, offset, "endstream");
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

		private readonly global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data.IndirectObjectOld containingObject;

		private readonly global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data.PdfDictionaryOld dictionary;

		private readonly global::Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader.Readers.PdfReader reader;

		private long position;

		private int length;
	}
}
