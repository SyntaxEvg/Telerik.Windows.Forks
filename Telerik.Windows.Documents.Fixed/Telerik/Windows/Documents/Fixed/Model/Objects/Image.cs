using System;
using System.Windows.Media.Imaging;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Resources;

namespace Telerik.Windows.Documents.Fixed.Model.Objects
{
	public class Image : PositionContentElement
	{
		public ImageSource ImageSource { get; set; }

		public double Width
		{
			get
			{
				if (this.width != null)
				{
					return this.width.Value;
				}
				if (this.ImageSource != null)
				{
					return (double)this.ImageSource.Width;
				}
				return 0.0;
			}
			set
			{
				this.width = new double?(value);
			}
		}

		public double Height
		{
			get
			{
				if (this.height != null)
				{
					return this.height.Value;
				}
				if (this.ImageSource != null)
				{
					return (double)this.ImageSource.Height;
				}
				return 0.0;
			}
			set
			{
				this.height = new double?(value);
			}
		}

		internal ColorBase StencilColor { get; set; }

		internal override FixedDocumentElementType ElementType
		{
			get
			{
				return FixedDocumentElementType.Image;
			}
		}

		public BitmapSource GetBitmapSource()
		{
			BitmapSource result = null;
			if (this.ImageSource != null)
			{
				if (this.StencilColor == null)
				{
					result = this.ImageSource.GetBitmapSource();
				}
				else
				{
					result = this.ImageSource.GetBitmapSource(this.StencilColor);
				}
			}
			return result;
		}

		internal override PositionContentElement CreateClonedInstance()
		{
			return new Image
			{
				width = this.width,
				height = this.height,
				ImageSource = this.ImageSource
			};
		}

		double? width;

		double? height;
	}
}
