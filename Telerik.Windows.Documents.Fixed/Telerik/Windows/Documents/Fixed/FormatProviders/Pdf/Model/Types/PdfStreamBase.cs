using System;
using System.IO;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.Keywords;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Converters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types
{
	abstract class PdfStreamBase : PdfPrimitive
	{
		public PdfStreamBase(IPdfImportContext context, Stream stream, PdfDictionary dictionary, PdfInt length, long offset)
		{
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Stream>(stream, "stream");
			Guard.ThrowExceptionIfNull<PdfDictionary>(dictionary, "dictionary");
			this.context = context;
			this.currentIndirectReference = context.CurrentIndirectReference;
			this.stream = stream;
			this.dictionary = dictionary;
			this.length = length;
			this.offset = offset;
			this.reader = new PostScriptReader(this.stream, new KeywordCollection());
		}

		public PdfPrimitive Filters
		{
			get
			{
				if (this.filters == null)
				{
					this.dictionary.TryGetElement<PdfPrimitive>(this.reader, this.context, "Filter", out this.filters);
				}
				return this.filters;
			}
		}

		public DecodeParameters[] DecodeParameters
		{
			get
			{
				if (this.decodeParameters == null)
				{
					this.decodeParameters = PdfStreamBase.ReadDecodeParameters(this.reader, this.context, this.dictionary);
				}
				return this.decodeParameters;
			}
		}

		public override PdfElementType Type
		{
			get
			{
				return PdfElementType.Stream;
			}
		}

		public PdfDictionary Dictionary
		{
			get
			{
				return this.dictionary;
			}
		}

		protected IndirectReference CurrentIndirectReference
		{
			get
			{
				return this.currentIndirectReference;
			}
		}

		protected PostScriptReader Reader
		{
			get
			{
				return this.reader;
			}
		}

		protected IPdfImportContext Context
		{
			get
			{
				return this.context;
			}
		}

		internal PdfInt Length
		{
			get
			{
				return this.length;
			}
		}

		internal Stream Stream
		{
			get
			{
				return this.stream;
			}
		}

		internal long Offset
		{
			get
			{
				return this.offset;
			}
		}

		public byte[] ReadRawPdfData()
		{
			if (this.rawPdfData == null)
			{
				this.rawPdfData = this.ReadRawPdfDataOverride();
			}
			return this.rawPdfData;
		}

		public byte[] ReadDecodedPdfData()
		{
			if (this.decodedPdfData == null)
			{
				this.decodedPdfData = this.DecodePdfData();
			}
			return this.decodedPdfData;
		}

		protected abstract byte[] ReadRawPdfDataOverride();

		protected abstract int CalculateLength(Reader reader);

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			this.dictionary.Write(writer, context);
			byte[] encodedData = this.ReadRawPdfData();
			PdfStreamObjectBase.WriteEncodedData(writer, context, encodedData);
		}

		internal byte[] CalculateStreamLengthAndReadData()
		{
			this.reader.Reader.Seek(this.offset, SeekOrigin.Begin);
			int count = this.CalculateLength(this.reader.Reader);
			this.reader.Reader.Seek(this.offset, SeekOrigin.Begin);
			return this.reader.Reader.Read(count);
		}

		byte[] DecodePdfData()
		{
			PdfArray pdfArray = this.Filters as PdfArray;
			if (pdfArray == null)
			{
				PdfName pdfName = this.Filters as PdfName;
				if (pdfName != null)
				{
					pdfArray = new PdfArray(new PdfName[] { pdfName });
				}
				else
				{
					pdfArray = new PdfArray(new PdfPrimitive[0]);
				}
			}
			return FiltersManager.Decode(this.reader, this.context, this.dictionary, this, pdfArray);
		}

		static DecodeParameters[] ReadDecodeParameters(PostScriptReader reader, IPdfImportContext context, PdfDictionary dictionary)
		{
			DecodeParameters[] result = null;
			PdfPrimitive primitive;
			if (dictionary.TryGetElement("DecodeParms", out primitive))
			{
				result = DecodeParametersConverter.Convert(reader, context, primitive);
			}
			return result;
		}

		readonly PdfDictionary dictionary;

		readonly PdfInt length;

		readonly long offset;

		readonly IPdfImportContext context;

		readonly Stream stream;

		byte[] rawPdfData;

		byte[] decodedPdfData;

		readonly IndirectReference currentIndirectReference;

		PdfPrimitive filters;

		readonly PostScriptReader reader;

		DecodeParameters[] decodeParameters;
	}
}
