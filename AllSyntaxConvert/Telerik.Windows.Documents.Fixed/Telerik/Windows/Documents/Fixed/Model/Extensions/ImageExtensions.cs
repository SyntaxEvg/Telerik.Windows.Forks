using System;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Primitives;

namespace Telerik.Windows.Documents.Fixed.Model.Extensions
{
	static class ImageExtensions
	{
		internal static ImageDataSource ToImageDataSource(this IImageDescriptor image)
		{
			bool flag = image.IsSimpleJpeg();
			if (flag)
			{
				byte[] encodedData = image.GetEncodedData();
				return new JpegImageDataSource(image.Width, image.Height, encodedData);
			}
			return image.ToDecodedImageDataSource();
		}

		internal static DecodedImageDataSource ToDecodedImageDataSource(this IImageDescriptor image)
		{
			DecodedImageDataSource result;
			if (image.SMask != null)
			{
				result = ImageExtensions.CalculateAlphaMaskedPixels(image);
			}
			else
			{
				IMask mask = image.Mask;
				PixelContainer pixels = image.ColorSpace.GetPixels(image, true);
				SizeI sizeI = ((mask != null) ? mask.GetMaskedImageSize(image) : new SizeI(image.Width, image.Height));
				result = new DecodedImageDataSource(sizeI.Width, sizeI.Height, pixels);
			}
			return result;
		}

		internal static DecodedImageDataSource ToDecodedImageDataSource(this IImageDescriptor image, Color stencilColor)
		{
			PixelContainer stencilMaskedPixels = ImageExtensions.GetStencilMaskedPixels(image, stencilColor);
			return new DecodedImageDataSource(image.Width, image.Height, stencilMaskedPixels);
		}

		internal static int GetScaledPixelIndex(this IImageDescriptor image, int scaledPixelX, int scaledPixelY, double scaleX, double scaleY)
		{
			int num = (int)((double)scaledPixelX * scaleX);
			int num2 = (int)((double)scaledPixelY * scaleY);
			return num2 * image.Width + num;
		}

		internal static bool IsSimpleJpeg(this IImageDescriptor image)
		{
			bool flag = image.Filters != null && image.Filters.Length == 1 && image.Filters[0] == "DCTDecode";
			if (flag && !ColorSpaceBase.HasMask(image))
			{
				bool flag2 = ImageExtensions.JpegHasInvertedColors(image);
				return !flag2;
			}
			return false;
		}

		static PixelContainer GetStencilMaskedPixels(IImageDescriptor image, Color stencilColor)
		{
			byte[] decodedData = image.GetDecodedData();
			if (decodedData == null)
			{
				return null;
			}
			int unmaskedColor = ImageMask.GetUnmaskedColor(image);
			int width = image.Width;
			int height = image.Height;
			ImageBitReader imageBitReader = new ImageBitReader(decodedData, 1, width);
			Color transparent = Color.Transparent;
			PixelContainer stencilImagePixelContainer = PixelContainerFactory.GetStencilImagePixelContainer(width, height, stencilColor, transparent);
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					int num = imageBitReader.Read();
					if (num == unmaskedColor)
					{
						stencilImagePixelContainer.Add(stencilColor);
					}
					else
					{
						stencilImagePixelContainer.Add(transparent);
					}
				}
			}
			return stencilImagePixelContainer;
		}

		static DecodedImageDataSource CalculateAlphaMaskedPixels(IImageDescriptor image)
		{
			IImageDescriptor smask = image.SMask;
			int num = Math.Max(image.Width, smask.Width);
			int num2 = Math.Max(image.Height, smask.Height);
			double imageScaleX = (double)image.Width / (double)num;
			double imageScaleY = (double)image.Height / (double)num2;
			double sMaskScaleX = (double)smask.Width / (double)num;
			double sMaskScaleY = (double)smask.Height / (double)num2;
			DecodedImageDataSource result;
			if (image.Width == smask.Width && image.Height == smask.Height)
			{
				result = ImageExtensions.GetImageSourceWhenSameImageSize(image, num2, num, imageScaleX, imageScaleY);
			}
			else
			{
				result = ImageExtensions.GetImageSourceWhenDifferentImageSize(image, num, num2, imageScaleX, imageScaleY, sMaskScaleX, sMaskScaleY);
			}
			return result;
		}

		static DecodedImageDataSource GetImageSourceWhenSameImageSize(IImageDescriptor image, int height, int width, double imageScaleX, double imageScaleY)
		{
			PixelContainer pixels = image.ColorSpace.GetPixels(image, false);
			PixelContainer pixels2 = image.SMask.ColorSpace.GetPixels(image.SMask, false);
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					int scaledPixelIndex = image.GetScaledPixelIndex(j, i, imageScaleX, imageScaleY);
					Color colorFromIndex = pixels2.GetColorFromIndex(scaledPixelIndex);
					Color colorFromIndex2 = pixels.GetColorFromIndex(scaledPixelIndex);
					colorFromIndex2.A = colorFromIndex.GetGrayComponent();
					pixels.SetColorToIndex(colorFromIndex2, scaledPixelIndex);
				}
			}
			return new DecodedImageDataSource(width, height, pixels);
		}

		static DecodedImageDataSource GetImageSourceWhenDifferentImageSize(IImageDescriptor image, int width, int height, double imageScaleX, double imageScaleY, double sMaskScaleX, double sMaskScaleY)
		{
			PixelContainer pixels = image.ColorSpace.GetPixels(image, false);
			PixelContainer pixels2 = image.SMask.ColorSpace.GetPixels(image.SMask, false);
			PixelContainer maskedImagePixelContainer = PixelContainerFactory.GetMaskedImagePixelContainer(width, height);
			for (int i = 0; i < height; i++)
			{
				int num = i * width;
				for (int j = 0; j < width; j++)
				{
					int scaledPixelIndex = image.GetScaledPixelIndex(j, i, imageScaleX, imageScaleY);
					int scaledPixelIndex2 = image.SMask.GetScaledPixelIndex(j, i, sMaskScaleX, sMaskScaleY);
					Color colorFromIndex = pixels.GetColorFromIndex(scaledPixelIndex);
					Color color = new Color(pixels2.GetColorFromIndex(scaledPixelIndex2).GetGrayComponent(), colorFromIndex.R, colorFromIndex.G, colorFromIndex.B);
					int index = num + j;
					maskedImagePixelContainer.SetColorToIndex(color, index);
				}
			}
			return new DecodedImageDataSource(width, height, maskedImagePixelContainer);
		}

		static bool JpegHasInvertedColors(IImageDescriptor image)
		{
			int componentCount = image.ColorSpace.ComponentCount;
			bool flag = componentCount == 4;
			if (flag)
			{
				return true;
			}
			if (image.Decode != null)
			{
				double[] defaultDecodeArray = image.ColorSpace.GetDefaultDecodeArray(image.BitsPerComponent);
				int num = System.Math.Min(defaultDecodeArray.Length, image.Decode.Length);
				for (int i = 0; i < num; i++)
				{
					double num2 = image.Decode[i];
					double num3 = defaultDecodeArray[i];
					if (num2 != num3)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
