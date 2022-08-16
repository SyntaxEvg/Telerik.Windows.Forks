using System;
using System.Collections.Generic;
using System.IO;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming
{
	class PdfFileStreamExportContext : PdfExportContext, IResourceRenamingExportContext
	{
		public PdfFileStreamExportContext(Stream stream, RadFixedDocumentInfo documentInfo, PdfExportSettings exportSettings)
			: base(documentInfo, exportSettings)
		{
			this.pagesRoot = new PageTreeNode();
			this.pagesRoot.Children = new PdfArray(new PdfPrimitive[0]);
			this.pagesRootObject = base.CreateIndirectObject(this.pagesRoot, false);
			this.writer = new PdfWriter(stream);
			this.sourceReferencesToContextReferencesMapping = new Dictionary<PdfFileSource, Dictionary<IndirectReference, IndirectReference>>();
			this.pageSourceToXFormReference = new Dictionary<PdfPageSource, IndirectReference>();
			this.pageRootToXFormReference = new InstanceIdCache<RadFixedPage, IndirectReference>();
			string message;
			if (RadFixedDocumentLicenseCheck.TryGetLicenseMessage(out message))
			{
				this.licenceMessageContentOwner = new RadFixedPage();
				RadFixedDocumentLicenseCheck.AddLicenceMessageContent(this.licenceMessageContentOwner, message);
			}
			this.hasFinalizedDocumentRoot = false;
		}

		public PdfWriter Writer
		{
			get
			{
				return this.writer;
			}
		}

		public RadFixedPage LicenceMessageContentOwner
		{
			get
			{
				return this.licenceMessageContentOwner;
			}
		}

		public IndirectReference AddNextPageReferenceToGetReference(Page page)
		{
			page.Parent = this.pagesRoot;
			this.pagesRoot.Children.Add(page);
			IndirectObject indirectObject = base.CreateIndirectObject(page, false);
			return indirectObject.Reference;
		}

		public DocumentCatalog GetFinalizedDocumentRoot()
		{
			Guard.ThrowExceptionIfTrue(this.hasFinalizedDocumentRoot, "hasFinalizedDocumentRoot");
			DocumentCatalog documentCatalog = new DocumentCatalog();
			documentCatalog.DocumentInfo = new DocumentInfo();
			documentCatalog.DocumentInfo.CopyPropertiesFrom(this);
			this.pagesRoot.Count = new PdfInt(this.pagesRoot.Children.Count);
			documentCatalog.Pages = this.pagesRoot;
			base.IndirectObjectsQueue.Enqueue(this.pagesRootObject);
			return documentCatalog;
		}

		public void AddXFormReference(PdfPageSource page, IndirectReference reference)
		{
			this.pageSourceToXFormReference.Add(page, reference);
		}

		public bool TryGetXFormReference(PdfPageSource page, out IndirectReference xFormReference)
		{
			return this.pageSourceToXFormReference.TryGetValue(page, out xFormReference);
		}

		public void AddXFormReference(RadFixedPage pageRoot, IndirectReference reference)
		{
			this.pageRootToXFormReference.Add(pageRoot, reference);
		}

		public bool TryGetXFormReference(RadFixedPage pageRoot, out IndirectReference xFormReference)
		{
			return this.pageRootToXFormReference.TryGetValue(pageRoot, out xFormReference);
		}

		public void AddPageSourceToContextReferenceMapping(PdfPageSource pageSource, IndirectReference contextPageReference)
		{
			Dictionary<IndirectReference, IndirectReference> referencesMapping = this.GetReferencesMapping(pageSource.FileSource);
			referencesMapping[pageSource.PageReference] = contextPageReference;
		}

		public void AddAnnotationDictionariesMappings(PdfPageSource pageSource, IndirectReference sourceAnnotationReference, IndirectReference contextAnnotationReference)
		{
			Dictionary<IndirectReference, IndirectReference> referencesMapping = this.GetReferencesMapping(pageSource.FileSource);
			referencesMapping[sourceAnnotationReference] = contextAnnotationReference;
		}

		public void AddSourceToContextReferenceMapping(PdfFileSource source, IndirectReference sourceReference, IndirectReference contextReference)
		{
			Dictionary<IndirectReference, IndirectReference> referencesMapping = this.GetReferencesMapping(source);
			referencesMapping.Add(sourceReference, contextReference);
		}

		public bool TryGetContextIndirectReference(PdfFileSource source, IndirectReference sourceReference, out IndirectReference contextReference)
		{
			Dictionary<IndirectReference, IndirectReference> referencesMapping = this.GetReferencesMapping(source);
			return referencesMapping.TryGetValue(sourceReference, out contextReference);
		}

		Dictionary<IndirectReference, IndirectReference> GetReferencesMapping(PdfFileSource source)
		{
			Guard.ThrowExceptionIfTrue(source.IsDisposed, "source.IsDisposed");
			Dictionary<IndirectReference, IndirectReference> dictionary;
			if (!this.sourceReferencesToContextReferencesMapping.TryGetValue(source, out dictionary))
			{
				dictionary = new Dictionary<IndirectReference, IndirectReference>();
				this.sourceReferencesToContextReferencesMapping.Add(source, dictionary);
				source.Disposed += this.PdfFileSourceDisposed;
			}
			return dictionary;
		}

		void PdfFileSourceDisposed(object sender, EventArgs e)
		{
			PdfFileSource pdfFileSource = (PdfFileSource)sender;
			pdfFileSource.Disposed -= this.PdfFileSourceDisposed;
			this.ClearCache(pdfFileSource);
		}

		void ClearCache(PdfFileSource fileSource)
		{
			this.sourceReferencesToContextReferencesMapping.Remove(fileSource);
			foreach (PdfPageSource key in fileSource.Pages)
			{
				this.pageSourceToXFormReference.Remove(key);
			}
		}

		readonly PdfWriter writer;

		readonly Dictionary<PdfFileSource, Dictionary<IndirectReference, IndirectReference>> sourceReferencesToContextReferencesMapping;

		readonly Dictionary<PdfPageSource, IndirectReference> pageSourceToXFormReference;

		readonly InstanceIdCache<RadFixedPage, IndirectReference> pageRootToXFormReference;

		readonly PageTreeNode pagesRoot;

		readonly IndirectObject pagesRootObject;

		readonly RadFixedPage licenceMessageContentOwner;

		bool hasFinalizedDocumentRoot;
	}
}
