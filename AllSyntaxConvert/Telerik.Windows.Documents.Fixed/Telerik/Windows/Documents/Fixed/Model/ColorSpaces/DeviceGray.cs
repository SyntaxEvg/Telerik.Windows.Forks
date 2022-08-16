using System;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Common.Functions;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.ColorSpaces
{
	class DeviceGray : ColorSpaceBase
	{
		public override int ComponentCount
		{
			get
			{
				return 1;
			}
		}

		public override string Name
		{
			get
			{
				return "DeviceGray";
			}
		}

		public override double[] GetDefaultDecodeArray(int bitsPerComponent)
		{
			return DeviceGray.GetDefaultDecodeArray();
		}

		public override PixelContainer GetPixels(IImageDescriptor image, bool applyMask)
		{
			return DeviceGray.GetGrayPixels(image, applyMask);
		}

		internal static PixelContainer GetGrayPixels(IImageDescriptor image, bool applyMask)
		{
			int bitsPerComponent = image.BitsPerComponent;
			byte[] decodedData = image.GetDecodedData();
			ImageBitReader imageBitReader = new ImageBitReader(decodedData, bitsPerComponent, image.Width);
			bool hasMask = ColorSpaceBase.HasMask(image);
			PixelContainer grayImagePixelContainer = PixelContainerFactory.GetGrayImagePixelContainer(bitsPerComponent, hasMask, image.Width, image.Height);
			double[] decodeArray = ColorSpaceBase.GetDecodeArray(image);
			double yMin = ColorSpaceBase.GetDecodeArrayComponent(decodeArray, 0);
			double yMax = ColorSpaceBase.GetDecodeArrayComponent(decodeArray, 1);
			double xMin = 0.0;
			double xMax = Math.Pow(2.0, (double)bitsPerComponent) - 1.0;
			IMask mask = image.Mask;
			bool shouldMaskColorComponents = ColorSpaceBase.ShouldMaskColorComponents(mask, applyMask);
			Func<int[], Color> getPixel = (int[] componentsArray) => Color.FromGray(FunctionBase.Interpolate((double)componentsArray[0], xMin, xMax, yMin, yMax));
			int[] array = new int[1];
			for (int i = 0; i < image.Height; i++)
			{
				for (int j = 0; j < image.Width; j++)
				{
					array[0] = imageBitReader.Read();
					grayImagePixelContainer.Add(ColorSpaceBase.GetPixelFromComponents(array, getPixel, mask, shouldMaskColorComponents));
				}
			}
			return ColorSpaceBase.GetPixelArray(image, grayImagePixelContainer, applyMask);
		}

		internal static double[] GetDefaultDecodeArray()
		{
			return ColorSpaceBase.GetDecodeArrayFromComponents(1);
		}

		internal override ColorBase GetColor(double[] pars)
		{
			Guard.ThrowExceptionIfNull<double[]>(pars, "pars");
			double gray = pars[0];
			Color color = Color.FromGray(gray);
			return new RgbColor(color);
		}

		internal override ColorBase GetColorFromLookup(byte[] lookupData, int offset)
		{
			Guard.ThrowExceptionIfNull<byte[]>(lookupData, "lookupData");
			Color color = Color.FromGray(lookupData[offset]);
			return new RgbColor(color);
		}
	}
}
