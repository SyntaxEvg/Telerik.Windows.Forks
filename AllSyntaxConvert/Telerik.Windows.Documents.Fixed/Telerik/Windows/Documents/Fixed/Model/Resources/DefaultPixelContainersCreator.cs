using System;
using Telerik.Windows.Documents.Core.Imaging;

namespace Telerik.Windows.Documents.Fixed.Model.Resources
{
	class DefaultPixelContainersCreator : IPixelContainersCreator
	{
		public PixelContainer CreateBgraContainer(int width, int height)
		{
			return new BgraPixelContainer(height * width);
		}

		public PixelContainer CreateGrayContainer(int width, int height)
		{
			return new GrayPixelContainer(height * width);
		}

		public PixelContainer CreateIndexedContainer(int width, int height, Color[] palette)
		{
			return new IndexedPixelContainer(height * width, palette);
		}

		public PixelContainer CreateMonochromeContainer(int width, int height, Color firstColor, Color secondColor)
		{
			return new MonochromePixelContainer(width, height, firstColor, secondColor);
		}
	}
}
