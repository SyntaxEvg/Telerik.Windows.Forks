using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.ContentElementWriters
{
	class StrokeColorWriter : ColorWriterBase
	{
		protected override void WriteRgbColor(PdfWriter writer, IPdfContentExportContext context, RgbColor rgbColor)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<RgbColor>(rgbColor, "rgbColor");
			if (rgbColor.A != 255)
			{
				ExtGState extGState = new ExtGState
				{
					StrokeAlphaConstant = new double?((double)rgbColor.A / 255.0),
					AlphaSource = new bool?(false)
				};
				ContentStreamOperators.SetGraphicsStateDictionaryOperator.Write(writer, context, context.GetResource(extGState).ResourceKey);
			}
			ContentStreamOperators.SetStrokeRgbColorOperator.Write(writer, context, rgbColor);
		}

		protected override void WritePatternColor(PdfWriter writer, IPdfContentExportContext context, PatternColor color)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PatternColor>(color, "color");
			if (color.PatternType == PatternType.Tiling)
			{
				TilingBase tilingBase = color as TilingBase;
				if (tilingBase.PaintType == PaintType.Uncolored)
				{
					this.WriteUncoloredTiling(writer, context, tilingBase as UncoloredTiling);
					return;
				}
			}
			ContentStreamOperators.SetStrokeColorSpace.Write(writer, context, color.ColorSpace);
			ContentStreamOperators.SetStrokeColorN.Write(writer, context, context.GetResource(color).ResourceKey);
		}

		void WriteUncoloredTiling(PdfWriter writer, IPdfContentExportContext context, UncoloredTiling uncoloredTiling)
		{
			ResourceEntry resource = context.GetResource(uncoloredTiling.ColorSpace, uncoloredTiling.Color.ColorSpace);
			ContentStreamOperators.SetStrokeColorSpace.Write(writer, context, resource.ResourceKey);
			ColorSpaceObject colorSpaceObject = ColorSpaceManager.CreateColorSpaceObject(uncoloredTiling.Color.ColorSpace);
			ColorObjectBase color = colorSpaceObject.CreateColorObject(context, uncoloredTiling.Color);
			ResourceEntry resource2 = context.GetResource(uncoloredTiling);
			ContentStreamOperators.SetStrokeColorN.Write(writer, context, color, resource2.ResourceKey);
		}
	}
}
