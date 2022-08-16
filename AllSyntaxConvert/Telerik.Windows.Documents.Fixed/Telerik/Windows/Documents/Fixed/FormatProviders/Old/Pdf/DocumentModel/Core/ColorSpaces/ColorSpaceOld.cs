using System;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Fixed.Exceptions;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Internal;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.ColorSpaces
{
	abstract class ColorSpaceOld : PdfObjectOld, IColorSpace
	{
		public ColorSpaceOld(PdfContentManager contentManager)
			: base(contentManager)
		{
		}

		public abstract int ComponentCount { get; }

		public virtual Brush DefaultBrush
		{
			get
			{
				return new SolidColorBrush(Color.Black);
			}
		}

		public abstract ColorSpace Type { get; }

		public static object[] CreateComponents(ColorSpaceOld cs, byte component)
		{
			object[] array = new object[cs.ComponentCount];
			for (int i = 0; i < cs.ComponentCount; i++)
			{
				array[i] = component;
			}
			return array;
		}

		public static bool IsColorSpace(string name)
		{
			name = ColorSpaceOld.GetColorSpaceName(name);
			string key;
			switch (key = name)
			{
			case "DeviceGray":
			case "DeviceRGB":
			case "DeviceCMYK":
			case "CalCMYK":
			case "CalGray":
			case "CalRGB":
			case "ICCBased":
			case "Indexed":
			case "Pattern":
			case "Separation":
				return true;
			}
			return false;
		}

		public static ColorSpaceOld CreateColorSpace(PdfContentManager contentManager, ColorSpace value)
		{
			switch (value)
			{
			case ColorSpace.Gray:
				return new DeviceGrayOld(contentManager);
			case ColorSpace.RGB:
				return new DeviceRgbOld(contentManager);
			default:
				throw new NotSupportedException("Color space is not supported");
			}
		}

		public static ColorSpaceOld CreateColorSpace(PdfContentManager contentManager, string name)
		{
			name = ColorSpaceOld.GetColorSpaceName(name);
			string key;
			switch (key = name)
			{
			case "DeviceGray":
				return new DeviceGrayOld(contentManager);
			case "DeviceRGB":
				return new DeviceRgbOld(contentManager);
			case "DeviceCMYK":
			case "CalCMYK":
				return new DeviceCmykOld(contentManager);
			case "CalGray":
				return new CalGrayOld(contentManager);
			case "CalRGB":
				return new CalRgbOld(contentManager);
			case "ICCBased":
				return new ICCBasedOld(contentManager);
			case "Indexed":
				return new IndexedOld(contentManager);
			case "Pattern":
				return new PatternColorSpaceOld(contentManager);
			case "Separation":
				return new SeparationOld(contentManager);
			case "DeviceN":
				return new DeviceNOld(contentManager);
			}
			throw new NotSupportedColorSpaceException(name);
		}

		public static ColorSpaceOld CreateColorSpace(PdfContentManager contentManager, PdfArrayOld array)
		{
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<PdfArrayOld>(array, "array");
			PdfNameOld element = array.GetElement<PdfNameOld>(0);
			ColorSpaceOld colorSpaceOld = ColorSpaceOld.CreateColorSpace(contentManager, element.Value);
			ICCBasedOld iccbasedOld = colorSpaceOld as ICCBasedOld;
			if (iccbasedOld != null)
			{
				iccbasedOld.Profile = array.GetElement<ICCProfileOld>(1);
				return iccbasedOld;
			}
			IndexedOld indexedOld = colorSpaceOld as IndexedOld;
			if (indexedOld != null)
			{
				indexedOld.Init(array);
				return indexedOld;
			}
			SeparationOld separationOld = colorSpaceOld as SeparationOld;
			if (separationOld != null)
			{
				separationOld.Init(array);
				return separationOld;
			}
			DeviceNOld deviceNOld = colorSpaceOld as DeviceNOld;
			if (deviceNOld != null)
			{
				deviceNOld.Init(array);
				return deviceNOld;
			}
			PatternColorSpaceOld patternColorSpaceOld = colorSpaceOld as PatternColorSpaceOld;
			if (patternColorSpaceOld != null && array.Count > 1)
			{
				patternColorSpaceOld.UnderlyingColorSpace = array.GetElement<ColorSpaceOld>(1, Converters.ColorSpaceConverter);
				return patternColorSpaceOld;
			}
			return colorSpaceOld;
		}

		public abstract Brush GetBrush(PdfResourceOld resources, object[] pars);

		public abstract Color GetColor(object[] pars);

		public abstract Color GetColor(byte[] bytes, int offset);

		public virtual void Clear()
		{
		}

		public override string ToString()
		{
			return base.GetType().Name;
		}

		public PixelContainer GetPixels(IImageDescriptor image, bool applyMask)
		{
			PixelContainer pixelsOverride = this.GetPixelsOverride(image, applyMask);
			this.Clear();
			return pixelsOverride;
		}

		public abstract double[] GetDefaultDecodeArray(int bitsPerComponent);

		protected abstract PixelContainer GetPixelsOverride(IImageDescriptor image, bool applyMask);

		protected static Color GetGrayColor(object[] pars)
		{
			Guard.ThrowExceptionIfNull<object[]>(pars, "pars");
			double gray;
			Helper.UnboxDouble(pars[pars.Length - 1], out gray);
			return Color.FromGray(gray);
		}

		protected static Color GetRgbColor(object[] pars)
		{
			Guard.ThrowExceptionIfNull<object[]>(pars, "pars");
			int num = pars.Length;
			double red;
			Helper.UnboxDouble(pars[num - 3], out red);
			double green;
			Helper.UnboxDouble(pars[num - 2], out green);
			double blue;
			Helper.UnboxDouble(pars[num - 1], out blue);
			return Color.FromArgb(1.0, red, green, blue);
		}

		protected static Color GetCmykColor(object[] pars)
		{
			Guard.ThrowExceptionIfNull<object[]>(pars, "pars");
			int num = pars.Length;
			double cyan;
			Helper.UnboxDouble(pars[num - 4], out cyan);
			double magenta;
			Helper.UnboxDouble(pars[num - 3], out magenta);
			double yellow;
			Helper.UnboxDouble(pars[num - 2], out yellow);
			double black;
			Helper.UnboxDouble(pars[num - 1], out black);
			return Color.FromCmyk(cyan, magenta, yellow, black);
		}

		protected static Color GetGrayColor(byte[] bytes, int offset)
		{
			Guard.ThrowExceptionIfNull<byte[]>(bytes, "bytes");
			return Color.FromGray(bytes[offset]);
		}

		protected static Color GetRgbColor(byte[] bytes, int offset)
		{
			Guard.ThrowExceptionIfNull<byte[]>(bytes, "bytes");
			return Color.FromArgb(byte.MaxValue, bytes[offset], bytes[offset + 1], bytes[offset + 2]);
		}

		protected static Color GetCmykColor(byte[] bytes, int offset)
		{
			Guard.ThrowExceptionIfNull<byte[]>(bytes, "bytes");
			return Color.FromCmyk(bytes[offset], bytes[offset + 1], bytes[offset + 2], bytes[offset + 3]);
		}

		static string GetColorSpaceName(string name)
		{
			if (name != null)
			{
				if (name == "G")
				{
					return "DeviceGray";
				}
				if (name == "RGB")
				{
					return "DeviceRGB";
				}
				if (name == "CMYK")
				{
					return "DeviceCMYK";
				}
				if (name == "I")
				{
					return "Indexed";
				}
			}
			return name;
		}

		const string DeviceGray = "DeviceGray";

		const string DeviceGrayAbrv = "G";

		const string DeviceRgb = "DeviceRGB";

		const string DeviceRgbAbrv = "RGB";

		const string DeviceCmyk = "DeviceCMYK";

		const string DeviceCmykAbrv = "CMYK";

		const string CalGray = "CalGray";

		const string CalRgb = "CalRGB";

		const string CalCmyk = "CalCMYK";

		const string ICCBased = "ICCBased";

		const string Indexed = "Indexed";

		const string IndexedAbrv = "I";

		const string Pattern = "Pattern";

		const string Separation = "Separation";

		const string DeviceN = "DeviceN";
	}
}
