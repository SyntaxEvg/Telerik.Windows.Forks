using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Forms;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.InteractiveForms;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure
{
	class DocumentCatalog : PdfObject
	{
		public DocumentCatalog()
		{
			this.pages = base.RegisterReferenceProperty<PageTreeNode>(new PdfPropertyDescriptor("Pages", true, PdfPropertyRestrictions.MustBeIndirectReference));
			this.documentInfo = base.RegisterReferenceProperty<DocumentInfo>(new PdfPropertyDescriptor("Metadata", false, PdfPropertyRestrictions.MustBeIndirectReference));
			this.outputIntents = base.RegisterReferenceProperty<PdfArray>(new PdfPropertyDescriptor("OutputIntents", false));
			this.acroForm = base.RegisterReferenceProperty<AcroFormObject>(new PdfPropertyDescriptor("AcroForm", false, PdfPropertyRestrictions.MustBeIndirectReference));
		}

		public PageTreeNode Pages
		{
			get
			{
				return this.pages.GetValue();
			}
			set
			{
				this.pages.SetValue(value);
			}
		}

		public DocumentInfo DocumentInfo
		{
			get
			{
				return this.documentInfo.GetValue();
			}
			set
			{
				this.documentInfo.SetValue(value);
			}
		}

		public PdfArray OutputIntents
		{
			get
			{
				return this.outputIntents.GetValue();
			}
			set
			{
				this.outputIntents.SetValue(value);
			}
		}

		public AcroFormObject AcroForm
		{
			get
			{
				return this.acroForm.GetValue();
			}
			set
			{
				this.acroForm.SetValue(value);
			}
		}

		public void CopyPropertiesFrom(IRadFixedDocumentExportContext context)
		{
			Guard.ThrowExceptionIfNull<IRadFixedDocumentExportContext>(context, "context");
			bool flag = context.Document.AcroForm.FormFields.Count > 0;
			if (flag)
			{
				this.CopyAcroFormProperties(context);
				context.PrepareWidgetAppearancesForExport();
			}
			this.CopyPagePropertiesFrom(context);
			if (flag)
			{
				this.CopyFieldPropertiesFrom(context);
			}
			this.DocumentInfo = new DocumentInfo();
			this.DocumentInfo.CopyPropertiesFrom(context);
		}

		void CopyAcroFormProperties(IRadFixedDocumentExportContext context)
		{
			this.AcroForm = new AcroFormObject();
			this.AcroForm.NeedAppearance = new PdfBool(context.Document.AcroForm.ViewersShouldRecalculateWidgetAppearances);
			this.AcroForm.Resources = new PdfResource();
			context.AcroFormContentExportContext = context.CreateContentExportContext(this.AcroForm, new EmptyContentRoot());
		}

		void CopyFieldPropertiesFrom(IRadFixedDocumentExportContext context)
		{
			this.AcroForm.Fields = new FormFieldsTree();
			foreach (FormField formField in context.Document.AcroForm.FormFields)
			{
				FormFieldNode formFieldNode = context.CreateFormFieldObject(formField, true);
				formFieldNode.CopyFormField(formField, context);
				this.AcroForm.Fields.Add(formFieldNode);
				if (formField.FieldType == FormFieldType.Signature && this.AcroForm.SigFlags == null)
				{
					this.AcroForm.SetSignatureFlags();
				}
			}
		}

		void CopyPagePropertiesFrom(IRadFixedDocumentExportContext context)
		{
			this.Pages = new PageTreeNode();
			if (context.Settings.ComplianceLevel != PdfComplianceLevel.None)
			{
				this.OutputIntents = new PdfArray(new PdfPrimitive[]
				{
					new OutputIntent()
				});
			}
			PdfArray pdfArray = new PdfArray(new PdfPrimitive[0]);
			foreach (RadFixedPage fixedPage in context.Document.Pages)
			{
				context.MapPages(fixedPage, new Page
				{
					Parent = this.Pages
				});
			}
			foreach (RadFixedPage radFixedPage in context.Document.Pages)
			{
				Page page;
				context.TryGetPage(radFixedPage, out page);
				this.CopyRadFixedPageProperties(radFixedPage, page, context);
				pdfArray.Add(page);
			}
			this.Pages.Children = pdfArray;
			this.Pages.Count = new PdfInt();
			this.Pages.Count.Value = context.Document.Pages.Count;
		}

		void CopyRadFixedPageProperties(RadFixedPage source, Page destination, IRadFixedDocumentExportContext context)
		{
			if (context.Document.CacheManager != null)
			{
				using (context.Document.CacheManager.BeginUsingPage(source))
				{
					destination.CopyPropertiesFrom(context, source);
					return;
				}
			}
			destination.CopyPropertiesFrom(context, source);
		}

		public void CopyPropertiesTo(PostScriptReader reader, IRadFixedDocumentImportContext context)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IRadFixedDocumentImportContext>(context, "context");
			this.CopyFormFieldsTo(reader, context);
			this.CopyPagePropertiesTo(reader, context);
		}

		void CopyFormFieldsTo(PostScriptReader reader, IRadFixedDocumentImportContext context)
		{
			if (this.AcroForm != null && this.AcroForm.Fields != null)
			{
				context.Document.AcroForm.ViewersShouldRecalculateWidgetAppearances = this.AcroForm.NeedAppearance.Value;
				foreach (FormFieldNode formFieldNode in this.AcroForm.Fields)
				{
					FormField field = formFieldNode.ToFormField(reader, context, true);
					context.Document.AcroForm.FormFields.Add(field);
				}
			}
		}

		void CopyPagePropertiesTo(PostScriptReader reader, IRadFixedDocumentImportContext context)
		{
			List<Page> contextPages = context.Pages;
			IList<Page> list = this.Pages.EnumeratePages(reader, context);
			switch (context.ImportSettings.ReadingMode)
			{
			case ReadingMode.AllAtOnce:
				this.CopyAllPageProperties(reader, context, contextPages, list);
				return;
			case ReadingMode.OnDemand:
				this.CopyLoadOnDemandPageProperties(reader, context, contextPages, list);
				return;
			default:
				throw new InvalidOperationException("Unrecognized ReadingMode value - " + context.ImportSettings.ReadingMode);
			}
		}

		void CopyAllPageProperties(PostScriptReader reader, IRadFixedDocumentImportContext context, List<Page> contextPages, IList<Page> pages)
		{
			for (int i = 0; i < pages.Count; i++)
			{
				Page page = pages[i];
				RadFixedPage fixedPage = this.AddPage(context, contextPages, page, i);
				page.CopyPropertiesTo(reader, context, fixedPage);
				page.CopyPageContentTo(context, fixedPage);
			}
			for (int j = 0; j < pages.Count; j++)
			{
				Page page2 = pages[j];
				RadFixedPage fixedPage2 = context.GetFixedPage(page2);
				page2.CopyPageAnnotationsTo(context, fixedPage2);
			}
		}

		void CopyLoadOnDemandPageProperties(PostScriptReader reader, IRadFixedDocumentImportContext context, List<Page> contextPages, IList<Page> pages)
		{
			for (int i = 0; i < pages.Count; i++)
			{
				Page page = pages[i];
				RadFixedPage fixedPage = this.AddPage(context, contextPages, page, i);
				page.CopyPropertiesTo(reader, context, fixedPage);
			}
		}

		RadFixedPage AddPage(IRadFixedDocumentImportContext context, List<Page> contextPages, Page page, int pageIndex)
		{
			RadFixedPage radFixedPage = context.Document.Pages.AddPage();
			radFixedPage.PageNumber = pageIndex + 1;
			context.MapPages(page, radFixedPage);
			contextPages.Add(page);
			return radFixedPage;
		}

		public const string PagesName = "Pages";

		public const string MetadataName = "Metadata";

		public const string OutputIntentsName = "OutputIntents";

		public const string AcroFormName = "AcroForm";

		readonly ReferenceProperty<PageTreeNode> pages;

		readonly ReferenceProperty<DocumentInfo> documentInfo;

		readonly ReferenceProperty<PdfArray> outputIntents;

		readonly ReferenceProperty<AcroFormObject> acroForm;
	}
}
