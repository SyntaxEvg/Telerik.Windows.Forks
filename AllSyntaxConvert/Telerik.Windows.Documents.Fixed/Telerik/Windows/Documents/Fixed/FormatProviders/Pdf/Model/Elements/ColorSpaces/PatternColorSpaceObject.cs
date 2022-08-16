using System;
using System.Linq;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Patterns;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces
{
	class PatternColorSpaceObject : ColorSpaceObject
	{
		public override string Name
		{
			get
			{
				return "Pattern";
			}
		}

		public override ColorObjectBase DefaultColor
		{
			get
			{
				return null;
			}
		}

		public override ColorSpace Public
		{
			get
			{
				return ColorSpace.Pattern;
			}
		}

		public ColorSpaceObject UnderlyingColorSpace { get; set; }

		public override ColorObjectBase CreateColorObject(IPdfContentExportContext context, ColorBase color)
		{
			throw new NotImplementedException();
		}

		public override void CopyPropertiesFrom(ColorSpaceBase colorSpaceBase)
		{
			Pattern pattern = (Pattern)colorSpaceBase;
			if (pattern.UnderlyingColorSpace != null)
			{
				ColorSpaceObject colorSpaceObject = ColorSpaceManager.CreateColorSpaceObject(pattern.UnderlyingColorSpace.Name);
				colorSpaceObject.CopyPropertiesFrom(pattern.UnderlyingColorSpace);
				this.UnderlyingColorSpace = colorSpaceObject;
			}
		}

		public override ColorObjectBase GetColor(IPdfContentImportContext context, PostScriptReader reader, PdfArray components)
		{
			PdfName key = (PdfName)components[components.Count - 1];
			PatternColorObject patternColor = context.GetPatternColor(reader, key);
			if (this.UnderlyingColorSpace != null)
			{
				PdfArray components2 = new PdfArray(components.Take(components.Count - 1));
				ColorObjectBase color = this.UnderlyingColorSpace.GetColor(context, reader, components2);
				patternColor.UnderlyingColor = color;
			}
			return patternColor;
		}

		public override ColorSpaceBase ToColorSpace()
		{
			Pattern pattern = new Pattern();
			if (this.UnderlyingColorSpace != null)
			{
				pattern.UnderlyingColorSpace = this.UnderlyingColorSpace.ToColorSpace();
			}
			return pattern;
		}

		public override void ImportFromArray(PostScriptReader reader, IPdfImportContext context, PdfArray array)
		{
			ColorSpaceObject underlyingColorSpace;
			array.TryGetElement<ColorSpaceObject>(reader, context, 1, out underlyingColorSpace);
			this.UnderlyingColorSpace = underlyingColorSpace;
		}
	}
}
