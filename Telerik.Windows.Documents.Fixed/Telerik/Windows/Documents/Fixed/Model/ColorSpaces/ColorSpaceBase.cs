using System;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Fixed.Model.Resources;

namespace Telerik.Windows.Documents.Fixed.Model.ColorSpaces
{
	abstract class ColorSpaceBase : IColorSpace
	{
		public abstract int ComponentCount { get; }

		public abstract string Name { get; }

		public abstract double[] GetDefaultDecodeArray(int bitsPerComponent);

		public abstract PixelContainer GetPixels(IImageDescriptor image, bool applyMask);

		public override string ToString()
		{
			return this.Name;
		}

		protected static double GetDecodeArrayComponent(double[] decodeArray, int componentIndex)
		{
			double result;
			if (decodeArray != null && decodeArray.Length > componentIndex)
			{
				result = decodeArray[componentIndex];
			}
			else
			{
				int num = componentIndex & 1;
				result = (double)num;
			}
			return result;
		}

		protected static double[] GetDecodeArray(IImageDescriptor image)
		{
			double[] array = image.Decode;
			if (array == null)
			{
				array = image.ColorSpace.GetDefaultDecodeArray(image.BitsPerComponent);
			}
			return array;
		}

		protected static bool ShouldMaskColorComponents(IMask mask, bool applyMask)
		{
			return applyMask && mask != null && mask.IsColorKeyMask;
		}

		protected static Color GetPixelFromComponents(int[] components, Func<int[], Color> getPixel, IMask mask, bool shouldMaskColorComponents)
		{
			if (shouldMaskColorComponents && mask.ShouldMaskColorComponents(components))
			{
				return Color.Transparent;
			}
			return getPixel(components);
		}

		protected static PixelContainer GetPixelArray(IImageDescriptor image, PixelContainer pixels, bool applyMask)
		{
			IMask mask = image.Mask;
			if (applyMask && mask != null && !mask.IsColorKeyMask)
			{
				pixels = mask.MaskImage(image, pixels);
			}
			return pixels;
		}

		internal static bool HasMask(IImageDescriptor image)
		{
			return image.SMask != null || image.Mask != null;
		}

		internal static double[] GetDecodeArrayFromComponents(int numberOfComponents)
		{
			double[] array = new double[numberOfComponents * 2];
			for (int i = 1; i < array.Length; i += 2)
			{
				array[i] = 1.0;
			}
			return array;
		}

		internal abstract ColorBase GetColor(double[] pars);

		internal abstract ColorBase GetColorFromLookup(byte[] lookupData, int offset);
	}
}
