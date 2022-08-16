using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.Collections;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.InteractiveForms;
using Telerik.Windows.Documents.Fixed.Model.Internal;
using Telerik.Windows.Documents.Fixed.Model.Navigation;
using Telerik.Windows.Documents.Fixed.Selection;
using Telerik.Windows.Documents.Fixed.Text;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model
{
	public class RadFixedDocument : FixedDocumentElementBase, IFixedDocument, IDestinationContainer, IFixedDocumentElement
	{
		public RadFixedDocument()
		{
			this.acroForm = new AcroForm();
			this.pages = new PageCollection(this);
			this.destinations = new DestinationCollection(this);
			this.documentInfo = new RadFixedDocumentInfo();
			this.bookmarks = new BookmarksCollection(this);
			this.PageMode = PageMode.UseNone;
			this.SupportsSelection = false;
		}

		public RadFixedDocumentInfo DocumentInfo
		{
			get
			{
				return this.documentInfo;
			}
		}

		public PageCollection Pages
		{
			get
			{
				return this.pages;
			}
		}

		public IEnumerable<Annotation> Annotations
		{
			get
			{
				foreach (RadFixedPage page in this.Pages)
				{
					foreach (Annotation annotation in page.Annotations)
					{
						yield return annotation;
					}
				}
				yield break;
			}
		}

		public DestinationCollection Destinations
		{
			get
			{
				return this.destinations;
			}
		}

		public AcroForm AcroForm
		{
			get
			{
				return this.acroForm;
			}
		}

		public PageMode PageMode { get; set; }

		public void Merge(RadFixedDocument source)
		{
			Guard.ThrowExceptionIfNull<RadFixedDocument>(source, "source");
			RadFixedDocument radFixedDocument = source.Clone();
			while (radFixedDocument.Destinations.Count > 0)
			{
				Destination item = radFixedDocument.Destinations.First<Destination>();
				radFixedDocument.Destinations.Remove(item);
				this.Destinations.Add(item);
			}
			while (radFixedDocument.Pages.Count > 0)
			{
				RadFixedPage item2 = radFixedDocument.Pages.First<RadFixedPage>();
				radFixedDocument.Pages.Remove(item2);
				this.Pages.Add(item2);
			}
		}

		public RadFixedDocument Clone()
		{
			RadFixedDocument radFixedDocument = new RadFixedDocument();
			RadFixedDocumentCloneContext radFixedDocumentCloneContext = new RadFixedDocumentCloneContext();
			foreach (RadFixedPage originalPage in this.Pages)
			{
				radFixedDocument.Pages.Add(radFixedDocumentCloneContext.GetClonedPage(originalPage));
			}
			foreach (Destination originalDestination in this.Destinations)
			{
				radFixedDocument.destinations.Add(radFixedDocumentCloneContext.GetClonedDestination(originalDestination));
			}
			radFixedDocument.AcroForm.ViewersShouldRecalculateWidgetAppearances = this.AcroForm.ViewersShouldRecalculateWidgetAppearances;
			foreach (FormField formField in this.AcroForm.FormFields)
			{
				FormField clonedField = radFixedDocumentCloneContext.GetClonedField(formField);
				radFixedDocument.AcroForm.FormFields.Add(clonedField);
				foreach (Widget originalAnnotation in formField.Widgets)
				{
					Widget clonedWidget = (Widget)radFixedDocumentCloneContext.GetClonedAnnotation(originalAnnotation);
					clonedField.AddClonedWidget(clonedWidget);
				}
			}
			radFixedDocument.DocumentInfo.Author = this.DocumentInfo.Author;
			radFixedDocument.DocumentInfo.Description = this.DocumentInfo.Description;
			radFixedDocument.DocumentInfo.Title = this.DocumentInfo.Title;
			return radFixedDocument;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public event EventHandler<OnDocumentExceptionEventArgs> OnException;

		[EditorBrowsable(EditorBrowsableState.Never)]
		public TextSelection Selection
		{
			get
			{
				if (this.selection == null)
				{
					this.selection = new TextSelection(this.TextDocument);
				}
				return this.selection;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public TextPosition CaretPosition
		{
			get
			{
				if (this.caretPosition == null)
				{
					this.caretPosition = new TextPosition(this.TextDocument);
				}
				return this.caretPosition;
			}
		}

		internal TextDocument TextDocument { get; set; }

		internal bool SupportsSelection { get; set; }

		internal IPagesCacheManager CacheManager { get; set; }

		internal RadFixedDocumentInternal InternalDocument
		{
			get
			{
				return this.internalDocument;
			}
			set
			{
				if (this.internalDocument != value)
				{
					if (this.internalDocument != null)
					{
						this.internalDocument.OnException -= this.InternalDocument_OnException;
					}
					this.internalDocument = value;
					if (this.internalDocument != null)
					{
						this.internalDocument.OnException += this.InternalDocument_OnException;
					}
				}
			}
		}

		void InternalDocument_OnException(object sender, OnDocumentExceptionEventArgs e)
		{
			if (this.OnException != null)
			{
				this.OnException(this, e);
			}
		}

		internal override FixedDocumentElementType ElementType
		{
			get
			{
				return FixedDocumentElementType.RadFixedDocument;
			}
		}

		internal BookmarksCollection Bookmarks
		{
			get
			{
				return this.bookmarks;
			}
		}

		readonly AcroForm acroForm;

		readonly PageCollection pages;

		readonly DestinationCollection destinations;

		readonly RadFixedDocumentInfo documentInfo;

		readonly BookmarksCollection bookmarks;

		TextSelection selection;

		TextPosition caretPosition;

		RadFixedDocumentInternal internalDocument;
	}
}
