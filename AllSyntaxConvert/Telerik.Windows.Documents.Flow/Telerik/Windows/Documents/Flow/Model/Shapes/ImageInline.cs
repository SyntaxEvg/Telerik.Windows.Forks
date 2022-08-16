using System;
using Telerik.Windows.Documents.Flow.Model.Cloning;
using Telerik.Windows.Documents.Model.Data;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Shapes
{
	public sealed class ImageInline : ShapeInlineBase
	{
		public ImageInline(RadFlowDocument document)
			: this(document, new Image())
		{
		}

		internal ImageInline(RadFlowDocument document, Image image)
			: base(document)
		{
			this.image = image;
			this.image.ImageSourceChanged += this.Image_ImageSourceChanged;
			if (this.image.ImageSource != null)
			{
				document.Resources.RegisterResource(this.image.ImageSource);
			}
		}

		public Image Image
		{
			get
			{
				return this.image;
			}
		}

		internal override DocumentElementType Type
		{
			get
			{
				return DocumentElementType.ImageInline;
			}
		}

		internal override ShapeBase Shape
		{
			get
			{
				return this.image;
			}
		}

		public ImageInline Clone()
		{
			return this.CloneInternal(null);
		}

		public ImageInline Clone(RadFlowDocument document)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			return this.CloneInternal(document);
		}

		internal override DocumentElementBase CloneCore(CloneContext cloneContext)
		{
			Guard.ThrowExceptionIfNull<CloneContext>(cloneContext, "cloneContext");
			Image image = this.Image.Clone(this.Document != cloneContext.Document);
			return new ImageInline(cloneContext.Document, image);
		}

		ImageInline CloneInternal(RadFlowDocument document = null)
		{
			return (ImageInline)this.CloneCore(new CloneContext(document ?? this.Document));
		}

		void Image_ImageSourceChanged(object sender, ResourceChangedEventArgs e)
		{
			if (e.OldValue != null)
			{
				this.Document.Resources.ReleaseResource(e.OldValue);
			}
			if (e.NewValue != null)
			{
				this.Document.Resources.RegisterResource(e.NewValue);
			}
		}

		readonly Image image;
	}
}
