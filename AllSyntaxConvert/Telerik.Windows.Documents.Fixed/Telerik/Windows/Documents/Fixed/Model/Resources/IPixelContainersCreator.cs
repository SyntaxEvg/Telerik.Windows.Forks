using System;
using Telerik.Windows.Documents.Core.Imaging;

namespace Telerik.Windows.Documents.Fixed.Model.Resources
{
	interface IPixelContainersCreator
	{
		PixelContainer CreateBgraContainer(int width, int height);

		PixelContainer CreateGrayContainer(int width, int height);

		PixelContainer CreateIndexedContainer(int width, int height, Color[] palette);

		PixelContainer CreateMonochromeContainer(int width, int height, Color firstColor, Color secondColor);
	}
}
