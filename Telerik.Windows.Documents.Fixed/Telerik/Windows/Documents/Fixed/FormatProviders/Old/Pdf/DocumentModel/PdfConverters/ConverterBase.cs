using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters
{
	abstract class ConverterBase : IConverter
	{
		public virtual bool HandlesIndirectReference
		{
			get
			{
				return false;
			}
		}

		public virtual object Convert(Type type, PdfContentManager contentManager, object value)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			PdfDictionaryOld pdfDictionaryOld = value as PdfDictionaryOld;
			if (pdfDictionaryOld != null)
			{
				return this.ConvertFromPdfDictionary(type, contentManager, pdfDictionaryOld);
			}
			PdfNameOld pdfNameOld = value as PdfNameOld;
			if (pdfNameOld != null)
			{
				return this.ConvertFromPdfName(type, contentManager, pdfNameOld);
			}
			PdfDataStream pdfDataStream = value as PdfDataStream;
			if (pdfDataStream != null)
			{
				return this.ConvertFromPdfDataStream(type, contentManager, pdfDataStream);
			}
			PdfArrayOld pdfArrayOld = value as PdfArrayOld;
			if (pdfArrayOld != null)
			{
				return this.ConvertFromPdfArray(type, contentManager, pdfArrayOld);
			}
			PdfStringOld pdfStringOld = value as PdfStringOld;
			if (pdfStringOld != null)
			{
				return this.ConvertFromPdfString(type, contentManager, pdfStringOld);
			}
			return value;
		}

		protected virtual object ConvertFromPdfDictionary(Type type, PdfContentManager contentManager, PdfDictionaryOld dictionary)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<PdfDictionaryOld>(dictionary, "dictionary");
			PdfObjectOld pdfObjectOld = Activator.CreateInstance(type, new object[] { contentManager }) as PdfObjectOld;
			if (pdfObjectOld != null)
			{
				pdfObjectOld.Load(dictionary);
				return pdfObjectOld;
			}
			return dictionary;
		}

		protected virtual object ConvertFromPdfDataStream(Type type, PdfContentManager contentManager, PdfDataStream stream)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<PdfDataStream>(stream, "stream");
			PdfStreamOld pdfStreamOld = Activator.CreateInstance(type, new object[] { contentManager }) as PdfStreamOld;
			if (pdfStreamOld != null)
			{
				pdfStreamOld.Load(stream);
				return pdfStreamOld;
			}
			return stream;
		}

		protected virtual object ConvertFromPdfName(Type type, PdfContentManager contentManager, PdfNameOld name)
		{
			return name;
		}

		protected virtual object ConvertFromPdfString(Type type, PdfContentManager contentManager, PdfStringOld str)
		{
			return str;
		}

		protected virtual object ConvertFromPdfArray(Type type, PdfContentManager contentManager, PdfArrayOld array)
		{
			return array;
		}
	}
}
