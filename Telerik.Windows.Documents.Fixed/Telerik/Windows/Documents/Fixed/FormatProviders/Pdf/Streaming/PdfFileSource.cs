using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming
{
	public sealed class PdfFileSource : IDisposable
	{
		public PdfFileSource(Stream pdfFileStream)
			: this(pdfFileStream, false)
		{
		}

		public PdfFileSource(Stream pdfFileStream, bool leaveStreamOpen)
			: this(pdfFileStream, new PdfImportSettings(), leaveStreamOpen)
		{
		}

		public PdfFileSource(Stream pdfFileStream, PdfImportSettings importSettings, bool leaveStreamOpen)
		{
			Guard.ThrowExceptionIfNull<Stream>(pdfFileStream, "pdfFileStream");
			Guard.ThrowExceptionIfNull<PdfImportSettings>(importSettings, "importSettings");
			this.fileStream = pdfFileStream;
			this.leaveStreamOpen = leaveStreamOpen;
			this.context = new PdfSourceImportContext(importSettings);
			this.pageTreeReferences = new HashSet<IndirectReference>();
			this.disposeValidator = new DisposeValidator();
			this.Context.BeginImport(pdfFileStream);
		}

		public PdfPageSource[] Pages
		{
			get
			{
				if (this.pages == null)
				{
					this.pages = this.LoadPages();
				}
				return this.pages;
			}
		}

		internal PdfFormFieldSourceCollection FormFields
		{
			get
			{
				if (this.formFields == null)
				{
					this.formFields = new PdfFormFieldSourceCollection(this);
				}
				return this.formFields;
			}
		}

		internal PdfSourceImportContext Context
		{
			get
			{
				return this.context;
			}
		}

		internal bool IsDisposed
		{
			get
			{
				return this.disposeValidator.IsDisposed;
			}
		}

		public void Dispose()
		{
			try
			{
				this.disposeValidator.Dispose();
				this.OnDisposed();
			}
			finally
			{
				if (!this.leaveStreamOpen)
				{
					this.fileStream.Dispose();
				}
			}
		}

		internal event EventHandler Disposed;

		internal bool IsPageTreeReference(IndirectReference reference)
		{
			return this.pageTreeReferences.Contains(reference);
		}

		PdfPageSource[] LoadPages()
		{
			DocumentCatalog value = this.Context.Root.GetValue();
			IEnumerable<PdfPageSource> source = this.EnumeratePageSources(value.Pages);
			return source.ToArray<PdfPageSource>();
		}

		IEnumerable<PdfPageSource> EnumeratePageSources(PageTreeNode pageNode)
		{
			for (int i = 0; i < pageNode.Children.Count; i++)
			{
				PageTreeNode pageTreeNode;
				if (pageNode.Children.TryGetElement<PageTreeNode>(this.context.Reader, this.Context, i, out pageTreeNode))
				{
					if (pageTreeNode.PageTreeNodeType == PageTreeNodeType.Page)
					{
						Page page = (Page)pageTreeNode;
						IndirectReference reference = (IndirectReference)pageNode.Children[i];
						this.pageTreeReferences.Add(reference);
						this.Context.Pages.Add(page);
						yield return new PdfPageSource(this, reference, page);
					}
					else if (pageTreeNode.PageTreeNodeType == PageTreeNodeType.PageTreeNode)
					{
						foreach (PdfPageSource pageSource in this.EnumeratePageSources(pageTreeNode))
						{
							yield return pageSource;
						}
					}
				}
			}
			yield break;
		}

		void OnDisposed()
		{
			if (this.Disposed != null)
			{
				this.Disposed(this, EventArgs.Empty);
			}
		}

		readonly Stream fileStream;

		readonly bool leaveStreamOpen;

		PdfPageSource[] pages;

		PdfFormFieldSourceCollection formFields;

		readonly PdfSourceImportContext context;

		readonly HashSet<IndirectReference> pageTreeReferences;

		readonly DisposeValidator disposeValidator;
	}
}
