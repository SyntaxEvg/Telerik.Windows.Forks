using System;
using System.IO;
using System.Windows;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming
{
	public sealed class PdfStreamWriter : IDisposable
	{
		public PdfStreamWriter(Stream stream)
			: this(stream, false)
		{
		}

		public PdfStreamWriter(Stream stream, bool leaveStreamOpen)
		{
			this.stream = stream;
			this.leaveStreamOpen = leaveStreamOpen;
			PdfExportSettings exportSettings = new PdfExportSettings();
			RadFixedDocumentInfo documentInfo = new RadFixedDocumentInfo();
			this.context = new PdfFileStreamExportContext(stream, documentInfo, exportSettings);
			this.settings = new PdfStreamWriterSettings(exportSettings, documentInfo);
			this.disposeValidator = new DisposeValidator();
			this.BeginDocument();
		}

		public PdfStreamWriterSettings Settings
		{
			get
			{
				return this.settings;
			}
		}

		internal PdfFileStreamExportContext Context
		{
			get
			{
				return this.context;
			}
		}

		public void WritePage(PdfPageSource pageSource)
		{
			Page page = new Page();
			IndirectReference contextPageReference = this.context.AddNextPageReferenceToGetReference(page);
			this.context.AddPageSourceToContextReferenceMapping(pageSource, contextPageReference);
			using (PdfPageStreamWriter pdfPageStreamWriter = this.BeginPage(page, pageSource.MediaBox, pageSource.CropBox, pageSource.Rotation))
			{
				pdfPageStreamWriter.WriteContent(pageSource);
			}
		}

		public void WritePage(RadFixedPage page)
		{
			Page page2 = this.CreateNextPageEmptyPageObject();
			this.context.MapPages(page, page2);
			using (PdfPageStreamWriter pdfPageStreamWriter = this.BeginPage(page2, page.MediaBox, page.CropBox, page.Rotation))
			{
				pdfPageStreamWriter.WriteContent(page);
			}
		}

		public PdfPageStreamWriter BeginPage(Size pageSize)
		{
			return this.BeginPage(pageSize, Rotation.Rotate0);
		}

		public PdfPageStreamWriter BeginPage(Size pageSize, Rotation rotation)
		{
			Rect rect = new Rect(0.0, 0.0, pageSize.Width, pageSize.Height);
			return this.BeginPage(rect, rect, rotation);
		}

		public PdfPageStreamWriter BeginPage(Rect mediaBox, Rect cropBox, Rotation rotation)
		{
			Page emptyPageObject = this.CreateNextPageEmptyPageObject();
			return this.BeginPage(emptyPageObject, mediaBox, cropBox, rotation);
		}

		public void Dispose()
		{
			try
			{
				this.disposeValidator.Dispose();
				this.WriteAllPendingPdfObjects();
			}
			finally
			{
				if (!this.leaveStreamOpen)
				{
					this.stream.Dispose();
				}
			}
		}

		internal void EndWritingPage()
		{
			Guard.ThrowExceptionIfFalse(this.isWritingPage, "isWritingPage");
			this.isWritingPage = false;
		}

		void BeginDocument()
		{
			this.context.Writer.WriteDocumentStart();
		}

		PdfPageStreamWriter BeginPage(Page emptyPageObject, Rect mediaBox, Rect cropBox, Rotation rotation)
		{
			if (this.isWritingPage)
			{
				throw new InvalidOperationException("Cannot begin a new page before the previous page is written.");
			}
			this.isWritingPage = true;
			PdfPageStreamWriterContext pageContext = new PdfPageStreamWriterContext
			{
				PageObject = emptyPageObject,
				MediaBox = mediaBox,
				CropBox = cropBox,
				PageRotation = rotation,
				Settings = this.settings,
				ExportContext = this.context,
				DoOnPageWritingEndedAction = new Action(this.EndWritingPage)
			};
			return new PdfPageStreamWriter(pageContext);
		}

		Page CreateNextPageEmptyPageObject()
		{
			Page page = new Page();
			this.context.AddNextPageReferenceToGetReference(page);
			return page;
		}

		void WriteAllPendingPdfObjects()
		{
			DocumentCatalog finalizedDocumentRoot = this.context.GetFinalizedDocumentRoot();
			PdfExporter.WriteCatalogAndRelatedProperties(this.context.Writer, this.context, finalizedDocumentRoot);
			PdfExporter.WriteFontsFromContext(this.context.Writer, this.context);
			long position = this.context.Writer.Position;
			PdfExporter.WriteTrailerAndCrossReferenceTable(this.context.Writer, this.context, finalizedDocumentRoot, null);
			PdfExporter.WriteDocumentEnd(position, this.context.Writer, this.context);
		}

		readonly Stream stream;

		readonly bool leaveStreamOpen;

		readonly PdfFileStreamExportContext context;

		readonly PdfStreamWriterSettings settings;

		readonly DisposeValidator disposeValidator;

		bool isWritingPage;
	}
}
