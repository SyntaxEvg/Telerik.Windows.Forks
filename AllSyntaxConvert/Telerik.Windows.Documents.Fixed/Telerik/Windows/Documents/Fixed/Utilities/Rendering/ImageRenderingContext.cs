using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Objects;
using Telerik.Windows.Documents.Fixed.Model.Resources;

namespace Telerik.Windows.Documents.Fixed.Utilities.Rendering
{
	class ImageRenderingContext : ContentRenderingContext
	{
		internal ImageRenderingContext(Image image, Rect currentContainerBounds)
			: base(image, currentContainerBounds)
		{
			this.imageSource = image.ImageSource;
			this.renderingWidth = image.Width;
			this.renderingHeight = image.Height;
			this.stencilColor = image.StencilColor;
		}

		public ImageSource ImageSource
		{
			get
			{
				return this.imageSource;
			}
		}

		public double RenderingWidth
		{
			get
			{
				return this.renderingWidth;
			}
		}

		public double RenderingHeight
		{
			get
			{
				return this.renderingHeight;
			}
		}

		public ColorBase StencilColor
		{
			get
			{
				return this.stencilColor;
			}
		}

		ImageSource imageSource;

		double renderingWidth;

		double renderingHeight;

		ColorBase stencilColor;
	}
}
