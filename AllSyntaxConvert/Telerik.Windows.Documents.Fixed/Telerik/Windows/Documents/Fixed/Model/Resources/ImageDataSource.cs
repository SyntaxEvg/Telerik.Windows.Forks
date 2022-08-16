using System;

namespace Telerik.Windows.Documents.Fixed.Model.Resources
{
	abstract class ImageDataSource
	{
		public ImageDataSource(int width, int height)
		{
			this.Width = width;
			this.Height = height;
		}

		public abstract bool IsEmpty { get; }

		public abstract ImageDataType ImageDataType { get; }

		public int Width { get; set; }

		public int Height { get; set; }
	}
}
