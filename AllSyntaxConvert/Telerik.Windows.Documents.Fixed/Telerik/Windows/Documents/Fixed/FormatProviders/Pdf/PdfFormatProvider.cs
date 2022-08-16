using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Telerik.Windows.Documents.Common.FormatProviders;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Core.Shapes;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Annot;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Forms;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Navigation;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader.Parsers;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.Collections;
using Telerik.Windows.Documents.Fixed.Model.InteractiveForms;
using Telerik.Windows.Documents.Fixed.Model.Internal;
using Telerik.Windows.Documents.Fixed.Model.Internal.Classes;
using Telerik.Windows.Documents.Fixed.Model.Internal.Collections;
using Telerik.Windows.Documents.Fixed.Model.Navigation;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Fixed.Text;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf
{
	public class PdfFormatProvider : BinaryFormatProviderBase<RadFixedDocument>
	{
		public PdfFormatProvider(Stream sourceStream)
			: this(sourceStream, FormatProviderSettings.ReadAllAtOnce)
		{
		}

		public PdfFormatProvider(Stream sourceStream, FormatProviderSettings settings)
			: this(sourceStream, settings, true)
		{
		}

		internal PdfFormatProvider(Stream sourceStream, FormatProviderSettings settings, bool copyStreamForReadAllAtOnceMode)
			: this()
		{
			Guard.ThrowExceptionIfNull<Stream>(sourceStream, "sourceStream");
			PdfImporter.ValidateInputStreamCanReadAndSeek(sourceStream);
			this.settings = settings;
			if (copyStreamForReadAllAtOnceMode && this.settings.ReadingMode == ReadingMode.AllAtOnce)
			{
				this.stream = new MemoryStream();
				sourceStream.Seek(0L, SeekOrigin.Begin);
				sourceStream.CopyTo(this.stream);
			}
			else
			{
				this.stream = sourceStream;
			}
			this.stream.Seek(0L, SeekOrigin.Begin);
			this.document = null;
			this.pagesMapping = new Dictionary<PageOld, RadFixedPageInternal>();
			this.isOldPdfViewer = true;
		}

		internal DocumentChangesManager ChangesManager
		{
			get
			{
				return this.changesManager;
			}
		}

		internal event EventHandler<OnDocumentExceptionEventArgs> OnException;

		public RadFixedDocument Import()
		{
			if (!this.isOldPdfViewer)
			{
				throw new NotSupportedException("This method is only supported when initializing PdfFormatProvider through the PdfFormatProvider(Stream, FormatProviderSettings) constructor.");
			}
			this.acroFormCacheManager = new AcroFormCacheManager(this.ImportSettings);
			this.contentManager = new PdfContentManager(this, this.stream);
			RadFixedDocument radFixedDocument = this.acroFormCacheManager.FieldsAndWidgetsImportContext.Document;
			this.document = new RadFixedDocumentInternal(this, radFixedDocument);
			radFixedDocument.InternalDocument = this.document;
			PagesContentCache pagesContentCache = new PagesContentCache();
			ThreadSafePageContentManagerOld threadSafePageContentManagerOld = new ThreadSafePageContentManagerOld(this);
			radFixedDocument.CacheManager = new LoadOnDemandPagesCacheManager(pagesContentCache, threadSafePageContentManagerOld);
			this.changesManager = new DocumentChangesManager(radFixedDocument, this.stream);
			this.document.OnException += this.Document_OnException;
			this.LoadFormFields();
			this.RecalculateMissingWidgetAppearances();
			this.LoadPages();
			this.LoadBookmarks();
			this.LoadPageMode();
			this.document.OnException -= this.Document_OnException;
			radFixedDocument.SupportsSelection = true;
			radFixedDocument.TextDocument = this.document.TextDocument;
			return radFixedDocument;
		}

		internal ImageDataSource GetImageSource(Image image)
		{
			ImageDataSource result;
			try
			{
				Guard.ThrowExceptionIfNull<Image>(image, "image");
				if (image.ImageSourceKey.StencilColor != null)
				{
					result = this.contentManager.ResourceManager.GetImageSourceWithStencilColor(image.ImageSourceKey);
				}
				else
				{
					result = this.contentManager.ResourceManager.GetImageSource(image.ImageSourceKey);
				}
			}
			catch (Exception e)
			{
				this.OnDocumentException(e);
				result = null;
			}
			return result;
		}

		internal RadFixedPageInternal GetRadFixedPageFromPage(PageOld page)
		{
			Guard.ThrowExceptionIfNull<PageOld>(page, "page");
			return this.pagesMapping[page];
		}

		internal bool TryGetWidgetFromWidgetObject(WidgetOld source, out Widget widget)
		{
			return this.acroFormCacheManager.TryGetWidgetFromWidgetObject(source, out widget);
		}

		internal ContentCollection ParseOnlyTextRelatedContent(RadFixedPageInternal page)
		{
			ContentCollection contentCollection = this.ParsePageContent(page, true);
			contentCollection.Arrange(page.TransformMatrix);
			return contentCollection;
		}

		internal ContentCollection ParsePageContent(RadFixedPageInternal page)
		{
			return this.ParsePageContent(page, false);
		}

		ContentCollection ParsePageContent(RadFixedPageInternal page, bool skipNonTextRelatedContent)
		{
			ContentCollection contentCollection = new ContentCollection();
			try
			{
				PageOld pageFromRadFixedPage = this.GetPageFromRadFixedPage(page);
				Container container = new Container();
				container.Clip = PathGeometry.CreateRectangle(page.CropBox);
				if (pageFromRadFixedPage.Contents != null)
				{
					container.Content = this.GetPageContent(pageFromRadFixedPage, skipNonTextRelatedContent);
				}
				Matrix identity = Matrix.Identity;
				identity.Translate(-page.CropBox.X, -page.CropBox.Y);
				container.TransformMatrix = identity;
				contentCollection.AddChild(container);
			}
			catch (Exception e)
			{
				this.OnDocumentException(e);
			}
			return contentCollection;
		}

		internal void LoadPageAnnotations(RadFixedPage page)
		{
			PageOld pageFromRadFixedPage = this.GetPageFromRadFixedPage(page.InternalRadFixedPage);
			this.CopyAnnotationsFromPageSource(pageFromRadFixedPage, page.Annotations);
			page.InternalRadFixedPage.Arrange();
		}

		internal Stream GetStream()
		{
			return this.stream;
		}

		internal void RecalculateWidgetAppearancesOld(string fieldName)
		{
			FormField formField = this.document.PublicDocument.AcroForm.FormFields[fieldName];
			RadFixedDocument radFixedDocument = this.BuildDocumentWithRecalculatedWidgetAppearances(Enumerable.Repeat<FormField>(formField, 1));
			FormField formField2 = radFixedDocument.AcroForm.FormFields[fieldName];
			Widget[] array = formField2.Widgets.ToArray<Widget>();
			int num = 0;
			foreach (Widget widget in formField.Widgets)
			{
				Widget widget2 = array[num];
				widget.AppearanceOld = widget2.AppearanceOld;
				num++;
			}
		}

		static bool ShouldRecalculateWidgetAppearance(Widget widget, bool shouldRecalculateAllAppearances)
		{
			return widget.AppearanceOld == null || (shouldRecalculateAllAppearances && widget.WidgetContentType == WidgetContentType.VariableContent);
		}

		void Document_OnException(object sender, OnDocumentExceptionEventArgs e)
		{
			this.OnOnException(e);
		}

		void OnOnException(OnDocumentExceptionEventArgs args)
		{
			if (this.OnException != null)
			{
				this.OnException(this, args);
			}
		}

		void OnDocumentException(Exception e)
		{
			if (this.document != null)
			{
				OnDocumentExceptionEventArgs onDocumentExceptionEventArgs = new OnDocumentExceptionEventArgs(e);
				if (onDocumentExceptionEventArgs.Handle)
				{
					this.document.OnOnException(onDocumentExceptionEventArgs);
					return;
				}
			}
			throw e;
		}

		void CopyAnnotationsFromPageSource(PageOld source, AnnotationCollection annotations)
		{
			Guard.ThrowExceptionIfNull<PageOld>(source, "source");
			Guard.ThrowExceptionIfNull<AnnotationCollection>(annotations, "annotations");
			if (source.Annots != null)
			{
				for (int i = 0; i < source.Annots.Count; i++)
				{
					AnnotationOld element = source.Annots.GetElement<AnnotationOld>(i, Converters.AnnotConverter);
					if (element != null)
					{
						Annotation annotation = PdfElementToFixedElementTranslator.CreateAnnotation(element, source, this);
						if (annotation != null)
						{
							annotations.Add(annotation);
						}
					}
				}
			}
		}

		ContentCollection GetPageContent(PageOld source, bool skipNonTextRelatedContent)
		{
			Guard.ThrowExceptionIfNull<PageOld>(source, "source");
			Guard.ThrowExceptionIfNull<ContentStreamOld>(source.Contents, "source.Contents");
			ContentCollection contentCollection = new ContentCollection();
			ContentStreamParser contentStreamParser = new ContentStreamParser(this.contentManager, source.Contents.Data, source.Clip, source.Resources, source.Reference);
			contentStreamParser.OnException += this.ContentStreamParser_OnException;
			IEnumerable<IContentElement> enumerable = (skipNonTextRelatedContent ? contentStreamParser.ParseOnlyTextRelatedContent() : contentStreamParser.ParseContent());
			foreach (IContentElement contentElement in enumerable)
			{
				ContentElement contentElement2 = (ContentElement)contentElement;
				PdfElementToFixedElementTranslator.ToSLCoordinates(contentElement2);
				contentCollection.Add(contentElement2);
			}
			contentStreamParser.OnException -= this.ContentStreamParser_OnException;
			return contentCollection;
		}

		void ContentStreamParser_OnException(object sender, OnDocumentExceptionEventArgs e)
		{
			this.OnDocumentException(e.DocumentException);
		}

		void LoadFormFields()
		{
			if (this.contentManager.DocumentCatalog.AcroForm != null && this.contentManager.DocumentCatalog.AcroForm.Fields != null)
			{
				try
				{
					PdfImporter pdfImporter = new PdfImporter();
					pdfImporter.Import(this.stream, this.acroFormCacheManager.FieldsAndWidgetsImportContext);
					int num = 0;
					int num2 = 0;
					foreach (FormFieldNodeOld formFieldNodeOld in this.contentManager.DocumentCatalog.AcroForm.Fields)
					{
						FormField field = this.acroFormCacheManager.FieldsAndWidgetsImportContext.GetField(num);
						if (field.FieldType == FormFieldType.Signature)
						{
							PdfElementToFixedElementTranslator.InitializeSignatureField((SignatureField)field, this.contentManager, formFieldNodeOld);
						}
						this.contentManager.RegisterFormField(formFieldNodeOld.Reference, field);
						foreach (WidgetOld widgetOld in this.contentManager.GetChildWidgets(formFieldNodeOld))
						{
							Widget widget = this.acroFormCacheManager.FieldsAndWidgetsImportContext.GetWidget(num2);
							this.acroFormCacheManager.MapWidget(widgetOld, widget);
							PdfElementToFixedElementTranslator.ApplyCommonAnnotationProperties(widgetOld, null, widget);
							num2++;
						}
						num++;
					}
				}
				catch (Exception e)
				{
					this.OnDocumentException(e);
				}
			}
		}

		void RecalculateMissingWidgetAppearances()
		{
			bool shouldRecalculateAllAppearances = this.document.PublicDocument.AcroForm.ViewersShouldRecalculateWidgetAppearances;
			Widget widget;
			FormField[] array = (from field in this.document.PublicDocument.AcroForm.FormFields
				where field.Widgets.Any((Widget w) => PdfFormatProvider.ShouldRecalculateWidgetAppearance(w, shouldRecalculateAllAppearances))
				select field).ToArray<FormField>();
			if (array.Length > 0)
			{
				foreach (FormField formField in array)
				{
					using (IEnumerator<Widget> enumerator = (from w in formField.Widgets
						where PdfFormatProvider.ShouldRecalculateWidgetAppearance(w, shouldRecalculateAllAppearances)
						select w).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							widget = enumerator.Current;
							widget.InvalidateAppearance();
						}
					}
				}
				RadFixedDocument radFixedDocument = this.BuildDocumentWithRecalculatedWidgetAppearances(array);
				foreach (FormField formField2 in array)
				{
					FormField formField3 = radFixedDocument.AcroForm.FormFields[formField2.Name];
					Widget[] array4 = formField3.Widgets.ToArray<Widget>();
					int num = 0;
					foreach (Widget widget2 in formField2.Widgets)
					{
						if (PdfFormatProvider.ShouldRecalculateWidgetAppearance(widget2, shouldRecalculateAllAppearances))
						{
							Widget widget3 = array4[num];
							widget2.AppearanceOld = widget3.AppearanceOld;
						}
						num++;
					}
				}
			}
		}

		RadFixedDocument BuildDocumentWithRecalculatedWidgetAppearances(IEnumerable<FormField> fieldsToRegenerate)
		{
			RadFixedDocument radFixedDocument = new RadFixedDocument();
			foreach (FormField field in fieldsToRegenerate)
			{
				radFixedDocument.AcroForm.FormFields.Add(field);
			}
			RadFixedDocument result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				PdfExporter pdfExporter = new PdfExporter();
				int startObjectId = this.acroFormCacheManager.BiggestObjectId + 1;
				IRadFixedDocumentExportContext context = new FieldsAndWidgetsPropertiesExportContext(startObjectId, radFixedDocument, new PdfExportSettings());
				pdfExporter.Export(context, memoryStream);
				PdfFormatProvider pdfFormatProvider = new PdfFormatProvider(memoryStream);
				RadFixedDocument radFixedDocument2 = pdfFormatProvider.Import();
				this.acroFormCacheManager.BiggestObjectId = pdfFormatProvider.acroFormCacheManager.BiggestObjectId;
				result = radFixedDocument2;
			}
			return result;
		}

		void LoadPages()
		{
			this.document.Pages = new PagesCollection(this.GetPages(this.document));
		}

		IEnumerable<RadFixedPageInternal> GetPages(RadFixedDocumentInternal doc)
		{
			Guard.ThrowExceptionIfNull<RadFixedDocumentInternal>(doc, "doc");
			int num = 0;
			IEnumerable<PageOld> enumerable = this.contentManager.GetPages();
			if (enumerable == null)
			{
				return Enumerable.Empty<RadFixedPageInternal>();
			}
			int num2 = enumerable.Count<PageOld>();
			RadFixedPageInternal[] array = new RadFixedPageInternal[num2];
			this.pages = new PageOld[num2];
			foreach (PageOld pageOld in enumerable)
			{
				num++;
				RadFixedPageInternal radFixedPageInternal = PdfElementToFixedElementTranslator.CreateFixedPage(doc, pageOld);
				radFixedPageInternal.PublicPage.PageNumber = num;
				this.pages[num - 1] = pageOld;
				this.pagesMapping[pageOld] = radFixedPageInternal;
				array[num - 1] = radFixedPageInternal;
			}
			return array;
		}

		PageOld GetPageFromRadFixedPage(RadFixedPageInternal page)
		{
			Guard.ThrowExceptionIfNull<RadFixedPageInternal>(page, "page");
			return this.pages[page.PublicPage.PageNumber - 1];
		}

		void LoadBookmarks()
		{
			if (this.contentManager.DocumentCatalog.Outlines != null)
			{
				for (OutlineItemOld outlineItemOld = this.contentManager.DocumentCatalog.Outlines.First; outlineItemOld != null; outlineItemOld = outlineItemOld.Next)
				{
					try
					{
						BookmarkItem item = PdfElementToFixedElementTranslator.CreateBookmarkItemHierarchy(outlineItemOld, this);
						this.document.PublicDocument.Bookmarks.Add(item);
					}
					catch (Exception e)
					{
						this.OnDocumentException(e);
					}
				}
			}
		}

		void LoadPageMode()
		{
			PdfNameOld pageMode = this.contentManager.DocumentCatalog.PageMode;
			this.document.PublicDocument.PageMode = PdfElementToFixedElementTranslator.CreatePageMode(pageMode);
		}

		public PdfFormatProvider()
		{
			this.ImportSettings = new PdfImportSettings();
			this.ExportSettings = new PdfExportSettings();
		}

		public override IEnumerable<string> SupportedExtensions
		{
			get
			{
				return PdfFormatProvider.supportedExtensions;
			}
		}

		public PdfImportSettings ImportSettings { get; set; }

		public PdfExportSettings ExportSettings { get; set; }

		public override bool CanImport
		{
			get
			{
				return true;
			}
		}

		public override bool CanExport
		{
			get
			{
				return true;
			}
		}

		internal override bool ShouldDisposeMemoryStreamOnImport
		{
			get
			{
				return this.ImportSettings.ReadingMode == ReadingMode.AllAtOnce;
			}
		}

		protected override RadFixedDocument ImportOverride(Stream input)
		{
			Guard.ThrowExceptionIfNull<Stream>(input, "input");
			if (this.isOldPdfViewer)
			{
				throw new NotSupportedException("This method is only supported when initializing PdfFormatProvider through the PdfFormatProvider() constructor.");
			}
			PdfImporter pdfImporter = new PdfImporter();
			RadFixedDocumentImportContext radFixedDocumentImportContext = new RadFixedDocumentImportContext(this.ImportSettings);
			pdfImporter.Import(input, radFixedDocumentImportContext);
			RadFixedDocument radFixedDocument = radFixedDocumentImportContext.Document;
			if (this.ImportSettings.ReadingMode == ReadingMode.OnDemand)
			{
				PagesContentCache pagesContentCache = new PagesContentCache();
				PageContentManager pageContentLoader = new PageContentManager(radFixedDocumentImportContext);
				ThreadSafePageContentManager threadSafePageContentManager = new ThreadSafePageContentManager(pageContentLoader);
				radFixedDocument.CacheManager = new LoadOnDemandPagesCacheManager(pagesContentCache, threadSafePageContentManager);
			}
			else
			{
				radFixedDocument.CacheManager = new AllAtOncePagesCacheManager();
			}
			if (this.ImportSettings.IsPdfViewer)
			{
				this.ApplyPdfViewerRelatedProperties(input, radFixedDocument);
			}
			return radFixedDocument;
		}

		protected override void ExportOverride(RadFixedDocument document, Stream output)
		{
			Guard.ThrowExceptionIfNull<RadFixedDocument>(document, "document");
			Guard.ThrowExceptionIfNull<Stream>(output, "output");
			if (this.isOldPdfViewer)
			{
				throw new NotSupportedException("This method is only supported when initializing PdfFormatProvider through the PdfFormatProvider() constructor.");
			}
			using (new RadFixedDocumentLicenseCheck(document))
			{
				PdfExporter pdfExporter = new PdfExporter();
				pdfExporter.Export(new RadFixedDocumentExportContext(document, this.ExportSettings), output);
			}
		}

		void ApplyPdfViewerRelatedProperties(Stream input, RadFixedDocument document)
		{
			TextDocument textDocument = new TextDocument(document);
			document.TextDocument = textDocument;
			document.SupportsSelection = true;
		}

		readonly Dictionary<PageOld, RadFixedPageInternal> pagesMapping;

		readonly Stream stream;

		readonly FormatProviderSettings settings;

		readonly bool isOldPdfViewer;

		AcroFormCacheManager acroFormCacheManager;

		DocumentChangesManager changesManager;

		PdfContentManager contentManager;

		PageOld[] pages;

		RadFixedDocumentInternal document;

		static readonly string[] supportedExtensions = new string[] { ".pdf" };
	}
}
