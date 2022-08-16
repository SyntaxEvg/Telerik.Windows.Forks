using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces
{
	class DeviceGrayColorSpaceObject : PdfRealColorSpaceObject
	{
		public override string Name
		{
			get
			{
				return "DeviceGray";
			}
		}

		public override ColorObjectBase DefaultColor
		{
			get
			{
				return DeviceGrayColorSpaceObject.defaultRgbColor;
			}
		}

		public override ColorSpace Public
		{
			get
			{
				return ColorSpace.Gray;
			}
		}

		public override ColorObjectBase CreateColorObject(IPdfContentExportContext context, ColorBase color)
		{
			RgbColorObject rgbColorObject = new RgbColorObject();
			rgbColorObject.CopyPropertiesFrom(context, color as RgbColor);
			return rgbColorObject;
		}

		protected override ColorObjectBase GetColor(IPdfContentImportContext context, PostScriptReader reader, PdfReal[] components)
		{
			PdfReal g = components[0];
			return new GrayColorObject(g);
		}

		public override void ImportFromArray(PostScriptReader reader, IPdfImportContext context, PdfArray array)
		{
		}

		public override void CopyPropertiesFrom(ColorSpaceBase colorSpaceBase)
		{
		}

		public override ColorSpaceBase ToColorSpace()
		{
			return new DeviceGray();
		}

		static readonly RgbColorObject defaultRgbColor = new RgbColorObject(0.0, 0.0, 0.0);
	}
}
