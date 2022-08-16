using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.Annotations.EventArgs;
using Telerik.Windows.Documents.Fixed.Model.Internal.Collections;
using Telerik.Windows.Documents.Fixed.Model.Navigation;
using Telerik.Windows.Documents.Fixed.Search;
using Telerik.Windows.Documents.Fixed.Text.Internal;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Internal
{
	class RadFixedDocumentInternal
	{
		internal RadFixedDocumentInternal(PdfFormatProvider formatProvider, RadFixedDocument publicDocument)
		{
			Guard.ThrowExceptionIfNull<PdfFormatProvider>(formatProvider, "formatProvider");
			this.formatProvider = formatProvider;
			this.PublicDocument = publicDocument;
		}

		public event EventHandler<OnDocumentExceptionEventArgs> OnException;

		internal event EventHandler<AnnotationEventArgs> AnnotationClicked;

		public RadFixedDocument PublicDocument { get; set; }

		public PagesCollection Pages
		{
			get
			{
				return this.pages;
			}
			internal set
			{
				if (this.pages != value)
				{
					this.pages = value;
				}
			}
		}

		public IEnumerable<Destination> Destinations
		{
			get
			{
				List<Destination> list = new List<Destination>();
				foreach (RadFixedPageInternal radFixedPageInternal in this.Pages)
				{
					foreach (Annotation annotation in radFixedPageInternal.Annotations)
					{
						Link link = annotation as Link;
						if (link != null && link.Destination != null)
						{
							list.Add(link.Destination);
						}
					}
				}
				return list;
			}
		}

		public IEnumerable<Annotation> Annotations
		{
			get
			{
				List<Annotation> list = new List<Annotation>();
				foreach (RadFixedPageInternal radFixedPageInternal in this.Pages)
				{
					list.AddRange(radFixedPageInternal.Annotations);
				}
				return list;
			}
		}

		internal PdfFormatProvider FormatProvider
		{
			get
			{
				return this.formatProvider;
			}
		}

		internal TextDocumentOld TextDocument
		{
			get
			{
				if (this.textDocument == null)
				{
					this.textDocument = new TextDocumentOld(this);
				}
				return this.textDocument;
			}
		}

		internal TextSearch TextSearch
		{
			get
			{
				if (this.textSearch == null)
				{
					this.textSearch = new TextSearch(this.TextDocument);
				}
				return this.textSearch;
			}
		}

		internal void OnAnnotationClicked(object sender, AnnotationEventArgs e)
		{
			if (this.AnnotationClicked != null)
			{
				this.AnnotationClicked(sender, e);
			}
		}

		internal void OnOnException(OnDocumentExceptionEventArgs args)
		{
			if (this.OnException != null)
			{
				this.OnException(this, args);
			}
		}

		readonly PdfFormatProvider formatProvider;

		PagesCollection pages;

		TextDocumentOld textDocument;

		TextSearch textSearch;
	}
}
