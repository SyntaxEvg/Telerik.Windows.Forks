using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.ContentElementWriters
{
	class ColorWriter : ColorWriterBase
	{
		protected override void WriteRgbColor(PdfWriter writer, IPdfContentExportContext context, RgbColor color)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<RgbColor>(color, "color");
			if (color.A != 255)
			{
				ExtGState extGState = new ExtGState
				{
					AlphaConstant = new double?((double)color.A / 255.0),
					AlphaSource = new bool?(false)
				};
				ContentStreamOperators.SetGraphicsStateDictionaryOperator.Write(writer, context, context.GetResource(extGState).ResourceKey);
			}
			ContentStreamOperators.SetRgbColorOperator.Write(writer, context, color);
		}

		protected override void WritePatternColor(PdfWriter writer, IPdfContentExportContext context, PatternColor color)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PatternColor>(color, "color");
			if (color.PatternType == PatternType.Tiling)
			{
				TilingBase tilingBase = (TilingBase)color;
				if (tilingBase.PaintType == PaintType.Uncolored)
				{
					this.WriteUncoloredTiling(writer, context, (UncoloredTiling)tilingBase);
					return;
				}
			}
			ContentStreamOperators.SetColorSpace.Write(writer, context, color.ColorSpace);
			ContentStreamOperators.SetColorN.Write(writer, context, context.GetResource(color).ResourceKey);
		}

		void WriteUncoloredTiling(PdfWriter writer, IPdfContentExportContext context, UncoloredTiling uncoloredTiling)
		{
			ContentStreamOperators.SetColorSpace.Write(writer, context, context.GetResource(uncoloredTiling.ColorSpace, uncoloredTiling.Color.ColorSpace).ResourceKey);
			ColorSpaceObject colorSpaceObject = ColorSpaceManager.CreateColorSpaceObject(uncoloredTiling.Color.ColorSpace);
			ColorObjectBase color = colorSpaceObject.CreateColorObject(context, uncoloredTiling.Color);
			ResourceEntry resource = context.GetResource(uncoloredTiling);
			ContentStreamOperators.SetColorN.Write(writer, context, color, resource.ResourceKey);
		}
	}
}
