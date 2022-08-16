using System;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Common.Functions;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.ColorSpaces
{
	class Separation : ColorSpaceBase, IMulticomponentColorSpace, IColorSpace
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
				return "Separation";
			}
		}

		internal string ColorantName { get; set; }

		internal ColorSpaceBase AlternateColorSpace { get; set; }

		internal FunctionBase TintTransform { get; set; }

		public override double[] GetDefaultDecodeArray(int bitsPerComponent)
		{
			return Separation.GetDefaultDecodeArray();
		}

		public override PixelContainer GetPixels(IImageDescriptor image, bool applyMask)
		{
			return Separation.GetSeparationPixels(image, this, applyMask);
		}

		internal static PixelContainer GetSeparationPixels(IImageDescriptor image, IMulticomponentColorSpace cs, bool applyMask)
		{
			int bitsPerComponent = image.BitsPerComponent;
			byte[] decodedData = image.GetDecodedData();
			ImageBitReader imageBitReader = new ImageBitReader(decodedData, bitsPerComponent, image.Width);
			PixelContainer separationImagePixelContainer = PixelContainerFactory.GetSeparationImagePixelContainer(image.Width, image.Height);
			double[] decodeArray = ColorSpaceBase.GetDecodeArray(image);
			double yMin = ColorSpaceBase.GetDecodeArrayComponent(decodeArray, 0);
			double yMax = ColorSpaceBase.GetDecodeArrayComponent(decodeArray, 1);
			double xMin = 0.0;
			double xMax = Math.Pow(2.0, (double)bitsPerComponent) - 1.0;
			IMask mask = image.Mask;
			bool shouldMaskColorComponents = ColorSpaceBase.ShouldMaskColorComponents(mask, applyMask);
			Func<int[], Color> getPixel = delegate(int[] componentsArray)
			{
				double num = FunctionBase.Interpolate((double)componentsArray[0], xMin, xMax, yMin, yMax);
				return cs.GetColor(new double[] { num });
			};
			int[] array = new int[1];
			for (int i = 0; i < image.Height; i++)
			{
				for (int j = 0; j < image.Width; j++)
				{
					array[0] = imageBitReader.Read();
					separationImagePixelContainer.Add(ColorSpaceBase.GetPixelFromComponents(array, getPixel, mask, shouldMaskColorComponents));
				}
			}
			return ColorSpaceBase.GetPixelArray(image, separationImagePixelContainer, applyMask);
		}

		internal static double[] GetDefaultDecodeArray()
		{
			return ColorSpaceBase.GetDecodeArrayFromComponents(1);
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
			return this.GetColor(new double[] { (double)lookupData[offset] });
		}

		Color IMulticomponentColorSpace.GetColor(double[] components)
		{
			ColorBase color = this.GetColor(components);
			return color.ToColor();
		}
	}
}
