using System;
using Telerik.Windows.Documents.Core.Imaging;

namespace Telerik.Windows.Documents.Fixed.Model.Resources
{
	static class PixelContainerFactory
	{
		public static PixelContainer GetRgbImagePixelContainer(int width, int height)
		{
			return PixelContainerFactory.GetBgraPixelContainer(width, height);
		}

		public static PixelContainer GetCmykImagePixelContainer(int width, int height)
		{
			return PixelContainerFactory.GetBgraPixelContainer(width, height);
		}

		public static PixelContainer GetDeviceNImagePixelContainer(int width, int height)
		{
			return PixelContainerFactory.GetBgraPixelContainer(width, height);
		}

		public static PixelContainer GetSeparationImagePixelContainer(int width, int height)
		{
			return PixelContainerFactory.GetBgraPixelContainer(width, height);
		}

		public static PixelContainer GetMaskedImagePixelContainer(int width, int height)
		{
			return PixelContainerFactory.GetBgraPixelContainer(width, height);
		}

		public static PixelContainer GetStencilImagePixelContainer(int width, int height, Color firstColor, Color secondColor)
		{
			return PixelContainerFactory.PixelContainersCreator.CreateMonochromeContainer(width, height, firstColor, secondColor);
		}

		public static PixelContainer GetGrayImagePixelContainer(int bitsPerComponent, bool hasMask, int width, int height)
		{
			PixelContainer result;
			if (hasMask)
			{
				result = PixelContainerFactory.PixelContainersCreator.CreateBgraContainer(width, height);
			}
			else if (bitsPerComponent == 1)
			{
				result = PixelContainerFactory.PixelContainersCreator.CreateMonochromeContainer(width, height, MonochromePixelContainer.DefaultFirstColor, MonochromePixelContainer.DefaultSecondColor);
			}
			else
			{
				result = PixelContainerFactory.PixelContainersCreator.CreateGrayContainer(width, height);
			}
			return result;
		}

		public static PixelContainer GetIndexedImagePixelContainer(bool hasMask, int width, int height, Color[] palette)
		{
			PixelContainer result;
			if (hasMask)
			{
				result = PixelContainerFactory.PixelContainersCreator.CreateBgraContainer(width, height);
			}
			else
			{
				result = PixelContainerFactory.PixelContainersCreator.CreateIndexedContainer(width, height, palette);
			}
			return result;
		}

		static PixelContainer GetBgraPixelContainer(int width, int height)
		{
			return PixelContainerFactory.PixelContainersCreator.CreateBgraContainer(width, height);
		}

		public static IPixelContainersCreator PixelContainersCreator = new DefaultPixelContainersCreator();
	}
}
