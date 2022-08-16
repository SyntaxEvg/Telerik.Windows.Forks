using System;
using Telerik.Windows.Documents.Flow.Model.Cloning;
using Telerik.Windows.Documents.Model.Data;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Shapes
{
	public sealed class FloatingImage : ShapeAnchorBase
	{
		public FloatingImage(RadFlowDocument document)
			: this(document, new Image())
		{
		}

		internal FloatingImage(RadFlowDocument document, Image image)
			: base(document)
		{
			this.image = image;
			this.image.ImageSourceChanged += this.Image_ImageSourceChanged;
			if (this.image.ImageSource != null)
			{
				this.Document.Resources.RegisterResource(this.image.ImageSource);
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
				return DocumentElementType.FloatingImage;
			}
		}

		internal override ShapeBase Shape
		{
			get
			{
				return this.image;
			}
		}

		public FloatingImage Clone()
		{
			return this.CloneInternal(null);
		}

		public FloatingImage Clone(RadFlowDocument document)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			return this.CloneInternal(document);
		}

		internal override DocumentElementBase CloneCore(CloneContext cloneContext)
		{
			Guard.ThrowExceptionIfNull<CloneContext>(cloneContext, "cloneContext");
			Image image = this.Image.Clone(this.Document != cloneContext.Document);
			FloatingImage floatingImage = new FloatingImage(cloneContext.Document, image);
			floatingImage.ClonePropertiesFrom(this);
			return floatingImage;
		}

		FloatingImage CloneInternal(RadFlowDocument document = null)
		{
			return (FloatingImage)this.CloneCore(new CloneContext(document ?? this.Document));
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
