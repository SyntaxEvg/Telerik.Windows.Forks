using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.Exceptions;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces
{
	static class ColorSpaceManager
	{
		static ColorSpaceManager()
		{
			ColorSpaceManager.RegisterMapping("CalGray", ColorSpace.CalGray, () => new CalGrayColorSpaceObject());
			ColorSpaceManager.RegisterMapping("CalRGB", ColorSpace.CalRgb, () => new CalRgbColorSpaceObject());
			ColorSpaceManager.RegisterMapping("DeviceCMYK", ColorSpace.CMYK, () => new DeviceCmykColorSpaceObject());
			ColorSpaceManager.RegisterMapping("DeviceN", ColorSpace.DeviceN, () => new DeviceNColorSpaceObject());
			ColorSpaceManager.RegisterMapping("DeviceGray", ColorSpace.Gray, () => new DeviceGrayColorSpaceObject());
			ColorSpaceManager.RegisterMapping("ICCBased", ColorSpace.ICCBased, () => new IccBasedColorSpaceObject());
			ColorSpaceManager.RegisterMapping("Indexed", ColorSpace.Indexed, () => new IndexedColorSpaceObject());
			ColorSpaceManager.RegisterMapping("Lab", ColorSpace.Lab, () => new LabColorSpaceObject());
			ColorSpaceManager.RegisterMapping("Pattern", ColorSpace.Pattern, () => new PatternColorSpaceObject());
			ColorSpaceManager.RegisterMapping("DeviceRGB", ColorSpace.RGB, () => new DeviceRgbColorSpaceObject());
			ColorSpaceManager.RegisterMapping("Separation", ColorSpace.Separation, () => new SeparationColorSpaceObject());
		}

		public static bool IsColorSpaceName(string name)
		{
			return ColorSpaceManager.stringToColoSpaceEnumMapper.ContainsKey(name);
		}

		public static ColorSpaceObject CreateColorSpaceObject(string colorSpaceName)
		{
			ColorSpace? colorSpaceEnumValue = ColorSpaceManager.GetColorSpaceEnumValue(colorSpaceName);
			if (colorSpaceEnumValue != null)
			{
				return ColorSpaceManager.CreateColorSpaceObject(colorSpaceEnumValue.Value);
			}
			throw new NotSupportedColorSpaceException(colorSpaceName);
		}

		public static ColorSpaceObject CreateColorSpaceObject(ColorSpace enumValue)
		{
			Func<ColorSpaceObject> func;
			if (ColorSpaceManager.creators.TryGetValue(enumValue, out func))
			{
				return func();
			}
			throw new NotSupportedColorSpaceException("The provided color space is not supported.");
		}

		public static ColorSpaceObject CreateColorSpaceObject(ColorSpaceBase colorSpaceBase)
		{
			ColorSpace? colorSpaceEnumValue = ColorSpaceManager.GetColorSpaceEnumValue(colorSpaceBase.Name);
			if (colorSpaceEnumValue != null)
			{
				ColorSpaceObject colorSpaceObject = ColorSpaceManager.CreateColorSpaceObject(colorSpaceEnumValue.Value);
				if (colorSpaceObject != null)
				{
					colorSpaceObject.CopyPropertiesFrom(colorSpaceBase);
					return colorSpaceObject;
				}
			}
			throw new NotSupportedColorSpaceException(string.Format("The provided color space: {0} is not supported.", colorSpaceBase.Name));
		}

		static ColorSpace? GetColorSpaceEnumValue(string colorSpaceName)
		{
			ColorSpace value;
			if (ColorSpaceManager.stringToColoSpaceEnumMapper.TryGetValue(colorSpaceName, out value))
			{
				return new ColorSpace?(value);
			}
			return null;
		}

		static void RegisterMapping(string name, ColorSpace enumValue, Func<ColorSpaceObject> creator)
		{
			ColorSpaceManager.stringToColoSpaceEnumMapper.Add(name, enumValue);
			ColorSpaceManager.coloSpaceEnumToStringMapper.Add(enumValue, name);
			ColorSpaceManager.creators.Add(enumValue, creator);
		}

		static readonly Dictionary<string, ColorSpace> stringToColoSpaceEnumMapper = new Dictionary<string, ColorSpace>();

		static readonly Dictionary<ColorSpace, string> coloSpaceEnumToStringMapper = new Dictionary<ColorSpace, string>();

		static readonly Dictionary<ColorSpace, Func<ColorSpaceObject>> creators = new Dictionary<ColorSpace, Func<ColorSpaceObject>>();
	}
}
