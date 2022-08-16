using System;
using System.IO;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.ContentElementWriters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure
{
	class ContentStream : PdfConcatenatableDataStream
	{
		public ContentStream()
		{
		}

		internal ContentStream(byte[] data)
		{
			base.Data = data;
		}

		public static byte[] BuildContentData(IPdfExportContext context, IResourceHolder resourceHolder, IContentRootElement contentRootElement)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<IResourceHolder>(resourceHolder, "resourceHolder");
			Guard.ThrowExceptionIfNull<IContentRootElement>(contentRootElement, "contentRootElement");
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				PdfWriter writer = new PdfWriter(memoryStream);
				IPdfContentExportContext context2 = context.CreateContentExportContext(resourceHolder, contentRootElement);
				ContentElementWriterBase.WriteElement(writer, context2, contentRootElement);
				byte[] array = new byte[memoryStream.Length];
				memoryStream.Seek(0L, SeekOrigin.Begin);
				memoryStream.Read(array, 0, array.Length);
				result = array;
			}
			return result;
		}

		public static void ParseContentData(byte[] data, IRadFixedDocumentImportContext context, IResourceHolder resourceHolder, IContentRootElement contentRoot)
		{
			Guard.ThrowExceptionIfNull<byte[]>(data, "data");
			Guard.ThrowExceptionIfNull<IRadFixedDocumentImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<IResourceHolder>(resourceHolder, "resourceHolder");
			Guard.ThrowExceptionIfNull<IContentRootElement>(contentRoot, "contentRoot");
			if (data.Length > 0)
			{
				using (Stream stream = new MemoryStream(data))
				{
					ContentStreamInterpreter contentStreamInterpreter = new ContentStreamInterpreter(stream, new PdfContentImportContext(context, resourceHolder, contentRoot));
					contentStreamInterpreter.Execute();
				}
			}
		}

		public void CopyPropertiesFrom(IPdfExportContext context, IResourceHolder resourceHolder, IContentRootElement contentRootElement)
		{
			base.Data = ContentStream.BuildContentData(context, resourceHolder, contentRootElement);
		}

		public void CopyPropertiesTo(IRadFixedDocumentImportContext context, IResourceHolder resourceHolder, IContentRootElement contentRoot)
		{
			ContentStream.ParseContentData(base.Data, context, resourceHolder, contentRoot);
			base.Data = null;
		}

		internal override void Concat(PdfConcatenatableDataStream stream)
		{
			Guard.ThrowExceptionIfNull<PdfConcatenatableDataStream>(stream, "stream");
			bool concatWithWhiteSpace = true;
			base.Data = PdfConcatenatableDataStream.Concat(base.Data, stream.Data, concatWithWhiteSpace);
		}
	}
}
