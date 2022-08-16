using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Forms;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.GraphicsState;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Objects;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import
{
	class RadFixedDocumentImportContext : BaseImportContext, IRadFixedDocumentImportContext, IPdfImportContext
	{
		public RadFixedDocumentImportContext(PdfImportSettings settings)
			: base(settings)
		{
			this.document = new RadFixedDocument();
			this.fonts = new Dictionary<FontObject, FontBase>();
			this.imageSources = new Dictionary<ImageXObject, Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource>();
			this.formSources = new Dictionary<FormXObject, FormSource>();
			this.pagesToFixedPages = new Dictionary<Page, RadFixedPage>();
			this.fixedPagesToPages = new Dictionary<RadFixedPage, Page>();
			this.pdfPointToDipTransformations = new Dictionary<Page, Matrix>();
			this.extGStates = new Dictionary<ExtGStateObject, ExtGState>();
		}

		public RadFixedDocument Document
		{
			get
			{
				return this.document;
			}
		}

		public FontBase GetFont(PostScriptReader reader, FontObject font)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<FontObject>(font, "font");
			FontBase fontBase;
			if (!this.fonts.TryGetValue(font, out fontBase))
			{
				fontBase = font.ToFont(reader, this);
				this.fonts[font] = fontBase;
			}
			return fontBase;
		}

		public FormSource GetFormSource(PostScriptReader reader, FormXObject form)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<FormXObject>(form, "form");
			FormSource formSource;
			if (!this.formSources.TryGetValue(form, out formSource))
			{
				formSource = form.ToFormSource(reader, this);
				this.formSources[form] = formSource;
			}
			return formSource;
		}

		public Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource GetImageSource(PostScriptReader reader, ImageXObject image)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<ImageXObject>(image, "image");
			Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource imageSource;
			if (!this.imageSources.TryGetValue(image, out imageSource))
			{
				imageSource = image.ToImageSource();
				this.imageSources[image] = imageSource;
			}
			return imageSource;
		}

		public ExtGState GetExtGState(PostScriptReader reader, ExtGStateObject extGState)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<ExtGStateObject>(extGState, "extGState");
			ExtGState extGState2;
			if (!this.extGStates.TryGetValue(extGState, out extGState2))
			{
				extGState2 = extGState.ToExtGState();
				this.extGStates[extGState] = extGState2;
			}
			return extGState2;
		}

		public void MapPages(Page page, RadFixedPage fixedPage)
		{
			this.fixedPagesToPages[fixedPage] = page;
			this.pagesToFixedPages[page] = fixedPage;
			this.pdfPointToDipTransformations[page] = this.CalculatePdfPointToDipTransformation(page);
		}

		public RadFixedPage GetFixedPage(Page page)
		{
			return this.pagesToFixedPages[page];
		}

		public Page GetPage(RadFixedPage fixedPage)
		{
			return this.fixedPagesToPages[fixedPage];
		}

		public Matrix GetPdfPointToDipTransformation(Page page)
		{
			return this.pdfPointToDipTransformations[page];
		}

		public IPdfContentImportContext GetAcroFormContentImportContext()
		{
			if (this.acroFormContentImportContext == null)
			{
				AcroFormObject acroForm = base.Root.GetValue().AcroForm;
				if (acroForm != null)
				{
					this.acroFormContentImportContext = new PdfContentImportContext(this, acroForm, new EmptyContentRoot());
				}
			}
			return this.acroFormContentImportContext;
		}

		protected override void BeginImportOverride()
		{
			DocumentCatalog value = base.Root.GetValue();
			value.CopyPropertiesTo(base.Reader, this);
		}

		Matrix CalculatePdfPointToDipTransformation(Page page)
		{
			Rect rect = page.MediaBox.ToRect(base.Reader, this);
			return new Matrix(RadFixedDocumentImportContext.PointToDip, 0.0, 0.0, -RadFixedDocumentImportContext.PointToDip, 0.0, Unit.PointToDip(rect.Height));
		}

		static readonly double PointToDip = Unit.PointToDip(1.0);

		readonly RadFixedDocument document;

		readonly Dictionary<FontObject, FontBase> fonts;

		readonly Dictionary<ImageXObject, Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource> imageSources;

		readonly Dictionary<FormXObject, FormSource> formSources;

		readonly Dictionary<ExtGStateObject, ExtGState> extGStates;

		readonly Dictionary<Page, RadFixedPage> pagesToFixedPages;

		readonly Dictionary<RadFixedPage, Page> fixedPagesToPages;

		readonly Dictionary<Page, Matrix> pdfPointToDipTransformations;

		IPdfContentImportContext acroFormContentImportContext;
	}
}
