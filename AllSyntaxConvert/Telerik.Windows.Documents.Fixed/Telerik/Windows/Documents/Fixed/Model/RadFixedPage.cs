using System;
using System.ComponentModel;
using System.Windows;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.Collections;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Fixed.Model.Internal;

namespace Telerik.Windows.Documents.Fixed.Model
{
	public class RadFixedPage : FixedDocumentElementBase, IContentRootElement, IContainerElement, IFixedDocumentElement, IContextClonable<RadFixedPage>, IInstanceIdOwner, IFixedPage
	{
		public RadFixedPage()
			: this(null)
		{
		}

		internal RadFixedPage(RadFixedDocument parent)
			: base(parent)
		{
			this.id = InstanceIdGenerator.GetNextId();
			this.content = new ContentElementCollection(this);
			this.MediaBox = new Rect(0.0, 0.0, FixedDocumentDefaults.PageSize.Width, FixedDocumentDefaults.PageSize.Height);
			this.Rotation = Rotation.Rotate0;
		}

		public bool SupportsAnnotations
		{
			get
			{
				return true;
			}
		}

		public ContentElementCollection Content
		{
			get
			{
				return this.content;
			}
		}

		public AnnotationCollection Annotations
		{
			get
			{
				this.LoadAnnotations();
				return this.annotations;
			}
		}

		public Size Size
		{
			get
			{
				return this.MediaBox.Size;
			}
			set
			{
				this.MediaBox = new Rect(this.MediaBox.X, this.MediaBox.Y, value.Width, value.Height);
			}
		}

		public Rect MediaBox { get; set; }

		public Rect CropBox
		{
			get
			{
				if (this.cropBox != null)
				{
					return this.cropBox.Value;
				}
				return this.MediaBox;
			}
			set
			{
				this.cropBox = new Rect?(value);
			}
		}

		public Rotation Rotation { get; set; }

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Use Telerik.Windows.Documents.Fixed.Utilities.PageLayoutHelper GetActualWidth method instead.")]
		public double ActualWidth
		{
			get
			{
				if (this.Rotation == Rotation.Rotate0 || this.Rotation == Rotation.Rotate180)
				{
					return this.Size.Width;
				}
				return this.Size.Height;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Use Telerik.Windows.Documents.Fixed.Utilities.PageLayoutHelper GetActualHeight method instead.")]
		public double ActualHeight
		{
			get
			{
				if (this.Rotation == Rotation.Rotate0 || this.Rotation == Rotation.Rotate180)
				{
					return this.Size.Height;
				}
				return this.Size.Width;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool HasContent
		{
			get
			{
				return this.InternalRadFixedPage.HasContent;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public RadFixedDocument Document
		{
			get
			{
				return (RadFixedDocument)base.Parent;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Use RadFixedDocument.Pages.IndexOf(RadFixedPage) method instead.")]
		public int PageNo
		{
			get
			{
				return this.PageNumber;
			}
			set
			{
				this.PageNumber = value;
			}
		}

		internal int PageNumber { get; set; }

		internal bool HasLoadedAnnotations
		{
			get
			{
				return this.annotations != null;
			}
		}

		internal RadFixedPageInternal InternalRadFixedPage { get; set; }

		internal override FixedDocumentElementType ElementType
		{
			get
			{
				return FixedDocumentElementType.RadFixedPage;
			}
		}

		int IInstanceIdOwner.InstanceId
		{
			get
			{
				return this.id;
			}
		}

		internal void LoadAnnotations()
		{
			if (!this.HasLoadedAnnotations)
			{
				this.annotations = new AnnotationCollection(this);
				if (this.InternalRadFixedPage != null)
				{
					PdfFormatProvider formatProvider = this.InternalRadFixedPage.Document.FormatProvider;
					lock (formatProvider)
					{
						formatProvider.LoadPageAnnotations(this);
					}
				}
			}
		}

		RadFixedPage IContextClonable<RadFixedPage>.Clone(RadFixedDocumentCloneContext cloneContext)
		{
			RadFixedPage radFixedPage = new RadFixedPage();
			radFixedPage.Size = this.Size;
			radFixedPage.Rotation = this.Rotation;
			foreach (ContentElementBase contentElementBase in this.Content)
			{
				PositionContentElement positionContentElement = (PositionContentElement)contentElementBase;
				radFixedPage.Content.Add(positionContentElement.Clone(cloneContext));
			}
			foreach (Annotation originalAnnotation in this.Annotations)
			{
				radFixedPage.Annotations.Add(cloneContext.GetClonedAnnotation(originalAnnotation));
			}
			return radFixedPage;
		}

		readonly int id;

		readonly ContentElementCollection content;

		AnnotationCollection annotations;

		Rect? cropBox;
	}
}
