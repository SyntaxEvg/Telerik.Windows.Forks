using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.Objects;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Layout
{
	class ImageLayoutElement : GraphicBasedLayoutElementBase<Image>
	{
		public ImageLayoutElement(Image image, TextProperties textProperties)
			: base(image, new Size(image.Width, image.Height), textProperties)
		{
		}
	}
}
