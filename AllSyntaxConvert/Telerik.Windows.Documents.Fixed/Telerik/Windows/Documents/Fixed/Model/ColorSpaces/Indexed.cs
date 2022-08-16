using System;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Common.Functions;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.ColorSpaces
{
	class Indexed : ColorSpaceBase, IIndexedColorSpace, IColorSpace
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
				return "Indexed";
			}
		}

		public ColorBase DefaultColor
		{
			get
			{
				return new RgbColor(0, 0, 0);
			}
		}

		public ColorSpaceBase Base { get; set; }

		public int HiVal { get; set; }

		public byte[] Lookup { get; set; }

		public Color GetColor(int index)
		{
			ColorBase colorAt = this.GetColorAt(index);
			return colorAt.ToColor();
		}

		public Color[] GetAllColors()
		{
			return Indexed.GetAllColors(this);
		}

		public override double[] GetDefaultDecodeArray(int bitsPerComponent)
		{
			return Indexed.GetIndexedDecodeArray(bitsPerComponent);
		}

		public override PixelContainer GetPixels(IImageDescriptor image, bool applyMask)
		{
			return Indexed.GetIndexedPixels(image, this, applyMask);
		}

		internal static PixelContainer GetIndexedPixels(IImageDescriptor image, IIndexedColorSpace cs, bool applyMask)
		{
			int bitsPerComponent = image.BitsPerComponent;
			byte[] decodedData = image.GetDecodedData();
			ImageBitReader imageBitReader = new ImageBitReader(decodedData, bitsPerComponent, image.Width);
			bool hasMask = ColorSpaceBase.HasMask(image);
			PixelContainer indexedImagePixelContainer = PixelContainerFactory.GetIndexedImagePixelContainer(hasMask, image.Width, image.Height, cs.GetAllColors());
			double[] decodeArray = ColorSpaceBase.GetDecodeArray(image);
			double yMin = ColorSpaceBase.GetDecodeArrayComponent(decodeArray, 0);
			double yMax = ColorSpaceBase.GetDecodeArrayComponent(decodeArray, 1);
			double xMin = 0.0;
			double xMax = Math.Pow(2.0, (double)bitsPerComponent) - 1.0;
			IMask mask = image.Mask;
			bool shouldMaskColorComponents = ColorSpaceBase.ShouldMaskColorComponents(mask, applyMask);
			Func<int[], Color> getPixel = (int[] componentsArray) => cs.GetColor((int)FunctionBase.Interpolate((double)componentsArray[0], xMin, xMax, yMin, yMax));
			int[] array = new int[1];
			for (int i = 0; i < image.Height; i++)
			{
				for (int j = 0; j < image.Width; j++)
				{
					array[0] = imageBitReader.Read();
					indexedImagePixelContainer.Add(ColorSpaceBase.GetPixelFromComponents(array, getPixel, mask, shouldMaskColorComponents));
				}
			}
			return ColorSpaceBase.GetPixelArray(image, indexedImagePixelContainer, applyMask);
		}

		internal static double[] GetIndexedDecodeArray(int bitsPerComponent)
		{
			return new double[]
			{
				0.0,
				Math.Pow(2.0, (double)bitsPerComponent) - 1.0
			};
		}

		internal static Color[] GetAllColors(IIndexedColorSpace cs)
		{
			Color[] array = new Color[cs.HiVal + 1];
			for (int i = 0; i <= cs.HiVal; i++)
			{
				Color color = cs.GetColor(i);
				array[i] = color;
			}
			return array;
		}

		internal override ColorBase GetColor(double[] pars)
		{
			Guard.ThrowExceptionIfNull<double[]>(pars, "pars");
			int index = (int)pars[0];
			return this.GetColorAt(index);
		}

		internal override ColorBase GetColorFromLookup(byte[] lookupData, int offset)
		{
			throw new NotSupportedException("This method is not supported.");
		}

		ColorBase GetColorAt(int index)
		{
			if (this.Base == null || this.Lookup == null)
			{
				return this.DefaultColor;
			}
			if (index < 0)
			{
				index = 0;
			}
			if (index > this.HiVal)
			{
				index = this.HiVal;
			}
			ColorSpaceBase colorSpaceBase = this.Base;
			IccBased iccBased = colorSpaceBase as IccBased;
			if (iccBased != null)
			{
				colorSpaceBase = iccBased.GetCachedAlternateColorSpace();
			}
			return colorSpaceBase.GetColorFromLookup(this.Lookup, index * colorSpaceBase.ComponentCount);
		}
	}
}
