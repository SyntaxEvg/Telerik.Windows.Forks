using System;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Common.Functions;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.ColorSpaces
{
	class DeviceCmyk : ColorSpaceBase
	{
		public override int ComponentCount
		{
			get
			{
				return 4;
			}
		}

		public override string Name
		{
			get
			{
				return "DeviceCMYK";
			}
		}

		public override double[] GetDefaultDecodeArray(int bitsPerComponent)
		{
			return DeviceCmyk.GetDefaultDecodeArray();
		}

		public override PixelContainer GetPixels(IImageDescriptor image, bool applyMask)
		{
			return DeviceCmyk.GetCmykPixels(image, applyMask);
		}

		internal static PixelContainer GetCmykPixels(IImageDescriptor image, bool applyMask)
		{
			int bitsPerComponent = image.BitsPerComponent;
			byte[] decodedData = image.GetDecodedData();
			ImageBitReader imageBitReader = new ImageBitReader(decodedData, bitsPerComponent, image.Width);
			PixelContainer cmykImagePixelContainer = PixelContainerFactory.GetCmykImagePixelContainer(image.Width, image.Height);
			double[] decodeArray = ColorSpaceBase.GetDecodeArray(image);
			double minC = ColorSpaceBase.GetDecodeArrayComponent(decodeArray, 0);
			double maxC = ColorSpaceBase.GetDecodeArrayComponent(decodeArray, 1);
			double minM = ColorSpaceBase.GetDecodeArrayComponent(decodeArray, 2);
			double maxM = ColorSpaceBase.GetDecodeArrayComponent(decodeArray, 3);
			double minY = ColorSpaceBase.GetDecodeArrayComponent(decodeArray, 4);
			double maxY = ColorSpaceBase.GetDecodeArrayComponent(decodeArray, 5);
			double minK = ColorSpaceBase.GetDecodeArrayComponent(decodeArray, 6);
			double maxK = ColorSpaceBase.GetDecodeArrayComponent(decodeArray, 7);
			double minX = 0.0;
			double maxX = Math.Pow(2.0, (double)bitsPerComponent) - 1.0;
			IMask mask = image.Mask;
			bool shouldMaskColorComponents = ColorSpaceBase.ShouldMaskColorComponents(mask, applyMask);
			Func<int[], Color> getPixel = (int[] componentsArray) => Color.FromCmyk(FunctionBase.Interpolate((double)componentsArray[0], minX, maxX, minC, maxC), FunctionBase.Interpolate((double)componentsArray[1], minX, maxX, minM, maxM), FunctionBase.Interpolate((double)componentsArray[2], minX, maxX, minY, maxY), FunctionBase.Interpolate((double)componentsArray[3], minX, maxX, minK, maxK));
			int[] array = new int[4];
			for (int i = 0; i < image.Height; i++)
			{
				for (int j = 0; j < image.Width; j++)
				{
					array[0] = imageBitReader.Read();
					array[1] = imageBitReader.Read();
					array[2] = imageBitReader.Read();
					array[3] = imageBitReader.Read();
					cmykImagePixelContainer.Add(ColorSpaceBase.GetPixelFromComponents(array, getPixel, mask, shouldMaskColorComponents));
				}
			}
			return ColorSpaceBase.GetPixelArray(image, cmykImagePixelContainer, applyMask);
		}

		internal static double[] GetDefaultDecodeArray()
		{
			return ColorSpaceBase.GetDecodeArrayFromComponents(4);
		}

		internal override ColorBase GetColor(double[] pars)
		{
			Guard.ThrowExceptionIfNull<double[]>(pars, "pars");
			int num = pars.Length;
			double cyan = pars[num - 4];
			double magenta = pars[num - 3];
			double yellow = pars[num - 2];
			double black = pars[num - 1];
			Color color = Color.FromCmyk(cyan, magenta, yellow, black);
			return new RgbColor(color);
		}

		internal override ColorBase GetColorFromLookup(byte[] lookupData, int offset)
		{
			Guard.ThrowExceptionIfNull<byte[]>(lookupData, "lookupData");
			Color color = Color.FromCmyk(lookupData[offset], lookupData[offset + 1], lookupData[offset + 2], lookupData[offset + 3]);
			return new RgbColor(color);
		}
	}
}
