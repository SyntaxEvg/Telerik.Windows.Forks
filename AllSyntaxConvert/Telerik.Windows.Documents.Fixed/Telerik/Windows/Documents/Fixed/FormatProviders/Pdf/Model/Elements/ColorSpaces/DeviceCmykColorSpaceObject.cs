using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces
{
	class DeviceCmykColorSpaceObject : PdfRealColorSpaceObject
	{
		public override string Name
		{
			get
			{
				return "DeviceCMYK";
			}
		}

		public override ColorObjectBase DefaultColor
		{
			get
			{
				return DeviceCmykColorSpaceObject.defaultRgbColor;
			}
		}

		public override ColorSpace Public
		{
			get
			{
				return ColorSpace.CMYK;
			}
		}

		public override ColorObjectBase CreateColorObject(IPdfContentExportContext context, ColorBase color)
		{
			throw new NotImplementedException();
		}

		protected override ColorObjectBase GetColor(IPdfContentImportContext context, PostScriptReader reader, PdfReal[] components)
		{
			PdfReal c = components[0];
			PdfReal m = components[1];
			PdfReal y = components[2];
			PdfReal k = components[3];
			return new CmykColorObject(c, m, y, k);
		}

		public override ColorSpaceBase ToColorSpace()
		{
			return new DeviceCmyk();
		}

		public override void ImportFromArray(PostScriptReader reader, IPdfImportContext context, PdfArray array)
		{
		}

		public override void CopyPropertiesFrom(ColorSpaceBase colorSpaceBase)
		{
		}

		static readonly RgbColorObject defaultRgbColor = new RgbColorObject(0.0, 0.0, 0.0);
	}
}
