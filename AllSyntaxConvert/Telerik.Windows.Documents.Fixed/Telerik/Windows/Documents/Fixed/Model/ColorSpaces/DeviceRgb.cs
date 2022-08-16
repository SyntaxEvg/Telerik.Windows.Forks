using System;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Common.Functions;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.ColorSpaces
{
	class DeviceRgb : ColorSpaceBase
	{
		public override int ComponentCount
		{
			get
			{
				return 3;
			}
		}

		public override string Name
		{
			get
			{
				return "DeviceRGB";
			}
		}

		public override double[] GetDefaultDecodeArray(int bitsPerComponent)
		{
			return DeviceRgb.GetDefaultDecodeArray();
		}

		public override PixelContainer GetPixels(IImageDescriptor image, bool applyMask)
		{
			return DeviceRgb.GetRgbPixels(image, applyMask);
		}

		internal static PixelContainer GetRgbPixels(IImageDescriptor image, bool applyMask)
		{
			int bitsPerComponent = image.BitsPerComponent;
			byte[] decodedData = image.GetDecodedData();
			ImageBitReader imageBitReader = new ImageBitReader(decodedData, bitsPerComponent, image.Width);
			PixelContainer rgbImagePixelContainer = PixelContainerFactory.GetRgbImagePixelContainer(image.Width, image.Height);
			double[] decodeArray = ColorSpaceBase.GetDecodeArray(image);
			double minR = ColorSpaceBase.GetDecodeArrayComponent(decodeArray, 0);
			double maxR = ColorSpaceBase.GetDecodeArrayComponent(decodeArray, 1);
			double minG = ColorSpaceBase.GetDecodeArrayComponent(decodeArray, 2);
			double maxG = ColorSpaceBase.GetDecodeArrayComponent(decodeArray, 3);
			double minB = ColorSpaceBase.GetDecodeArrayComponent(decodeArray, 4);
			double maxB = ColorSpaceBase.GetDecodeArrayComponent(decodeArray, 5);
			double minX = 0.0;
			double maxX = Math.Pow(2.0, (double)bitsPerComponent) - 1.0;
			IMask mask = image.Mask;
			bool shouldMaskColorComponents = ColorSpaceBase.ShouldMaskColorComponents(mask, applyMask);
			Func<int[], Color> getPixel = (int[] componentsArray) => Color.FromArgb(1.0, FunctionBase.Interpolate((double)componentsArray[0], minX, maxX, minR, maxR), FunctionBase.Interpolate((double)componentsArray[1], minX, maxX, minG, maxG), FunctionBase.Interpolate((double)componentsArray[2], minX, maxX, minB, maxB));
			int[] array = new int[3];
			for (int i = 0; i < image.Height; i++)
			{
				for (int j = 0; j < image.Width; j++)
				{
					array[0] = imageBitReader.Read();
					array[1] = imageBitReader.Read();
					array[2] = imageBitReader.Read();
					rgbImagePixelContainer.Add(ColorSpaceBase.GetPixelFromComponents(array, getPixel, mask, shouldMaskColorComponents));
				}
			}
			return ColorSpaceBase.GetPixelArray(image, rgbImagePixelContainer, applyMask);
		}

		internal static double[] GetDefaultDecodeArray()
		{
			return ColorSpaceBase.GetDecodeArrayFromComponents(3);
		}

		internal override ColorBase GetColor(double[] pars)
		{
			Guard.ThrowExceptionIfNull<double[]>(pars, "pars");
			byte r = Color.ConvertColorComponentToByte(pars[0]);
			byte g = Color.ConvertColorComponentToByte(pars[1]);
			byte b = Color.ConvertColorComponentToByte(pars[2]);
			return new RgbColor(r, g, b);
		}

		internal override ColorBase GetColorFromLookup(byte[] lookupData, int offset)
		{
			Guard.ThrowExceptionIfNull<byte[]>(lookupData, "lookupData");
			Color color = Color.FromArgb(byte.MaxValue, lookupData[offset], lookupData[offset + 1], lookupData[offset + 2]);
			return new RgbColor(color);
		}
	}
}
