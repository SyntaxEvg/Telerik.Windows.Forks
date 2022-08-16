using System;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Common.Functions;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.ColorSpaces
{
	class DeviceN : ColorSpaceBase, IMulticomponentColorSpace, IColorSpace
	{
		public override int ComponentCount
		{
			get
			{
				return this.Names.Length;
			}
		}

		public override string Name
		{
			get
			{
				return "DeviceN";
			}
		}

		public string[] Names { get; set; }

		public ColorSpaceBase AlternateColorSpace { get; set; }

		public FunctionBase TintTransform { get; set; }

		public override PixelContainer GetPixels(IImageDescriptor image, bool applyMask)
		{
			return DeviceN.GetDeviceNPixels(image, this, applyMask);
		}

		public override double[] GetDefaultDecodeArray(int bitsPerComponent)
		{
			return DeviceN.GetDefaultDecodeArray(this);
		}

		internal static double[] GetDefaultDecodeArray(IMulticomponentColorSpace cs)
		{
			return ColorSpaceBase.GetDecodeArrayFromComponents(cs.ComponentCount);
		}

		internal static PixelContainer GetDeviceNPixels(IImageDescriptor image, IMulticomponentColorSpace cs, bool applyMask)
		{
			int componentsCount = cs.ComponentCount;
			int bitsPerComponent = image.BitsPerComponent;
			byte[] decodedData = image.GetDecodedData();
			ImageBitReader imageBitReader = new ImageBitReader(decodedData, bitsPerComponent, image.Width);
			PixelContainer deviceNImagePixelContainer = PixelContainerFactory.GetDeviceNImagePixelContainer(image.Width, image.Height);
			double[] decodeArray = ColorSpaceBase.GetDecodeArray(image);
			double[] yMin = new double[componentsCount];
			double[] yMax = new double[componentsCount];
			for (int i = 0; i < componentsCount; i++)
			{
				int num = 2 * i;
				int componentIndex = num + 1;
				yMin[i] = ColorSpaceBase.GetDecodeArrayComponent(decodeArray, num);
				yMax[i] = ColorSpaceBase.GetDecodeArrayComponent(decodeArray, componentIndex);
			}
			double xMin = 0.0;
			double xMax = Math.Pow(2.0, (double)bitsPerComponent) - 1.0;
			IMask mask = image.Mask;
			bool shouldMaskColorComponents = ColorSpaceBase.ShouldMaskColorComponents(mask, applyMask);
			Func<int[], Color> getPixel = delegate(int[] componentsArray)
			{
				double[] array2 = new double[componentsCount];
				for (int m = 0; m < componentsCount; m++)
				{
					array2[m] = FunctionBase.Interpolate((double)componentsArray[m], xMin, xMax, yMin[m], yMax[m]);
				}
				return cs.GetColor(array2);
			};
			int[] array = new int[componentsCount];
			for (int j = 0; j < image.Height; j++)
			{
				for (int k = 0; k < image.Width; k++)
				{
					for (int l = 0; l < componentsCount; l++)
					{
						array[l] = imageBitReader.Read();
					}
					deviceNImagePixelContainer.Add(ColorSpaceBase.GetPixelFromComponents(array, getPixel, mask, shouldMaskColorComponents));
				}
			}
			return ColorSpaceBase.GetPixelArray(image, deviceNImagePixelContainer, applyMask);
		}

		internal override ColorBase GetColor(double[] pars)
		{
			Guard.ThrowExceptionIfNull<double[]>(pars, "pars");
			double[] pars2 = this.TintTransform.Execute(pars);
			return this.AlternateColorSpace.GetColor(pars2);
		}

		internal override ColorBase GetColorFromLookup(byte[] lookupData, int offset)
		{
			Guard.ThrowExceptionIfNull<byte[]>(lookupData, "lookupData");
			double[] array = new double[this.ComponentCount];
			for (int i = 0; i < this.ComponentCount; i++)
			{
				array[i] = (double)lookupData[offset + i];
			}
			return this.GetColor(array);
		}

		Color IMulticomponentColorSpace.GetColor(double[] components)
		{
			ColorBase color = this.GetColor(components);
			return color.ToColor();
		}
	}
}
