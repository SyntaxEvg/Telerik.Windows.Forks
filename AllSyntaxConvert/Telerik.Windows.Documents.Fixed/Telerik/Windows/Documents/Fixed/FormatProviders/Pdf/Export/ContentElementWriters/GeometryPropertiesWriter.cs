using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators;
using Telerik.Windows.Documents.Fixed.Model;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.ContentElementWriters
{
	class GeometryPropertiesWriter : ContentElementWriter<GeometryPropertiesOwner>
	{
		protected override void WriteOverride(PdfWriter writer, IPdfContentExportContext context, GeometryPropertiesOwner element)
		{
			if (element.IsFilled)
			{
				ContentElementWriters.ColorWriter.Write(writer, context, element.Fill);
			}
			if (element.IsStroked)
			{
				ContentElementWriters.StrokeColorWriter.Write(writer, context, element.Stroke);
				ContentStreamOperators.SetLineWidthOperator.Write(writer, context, element.StrokeThickness);
				ContentStreamOperators.SetLineJoinOperator.Write(writer, context, element.StrokeLineJoin);
				ContentStreamOperators.SetLineCapOperator.Write(writer, context, element.StrokeLineCap);
				if (element.StrokeDashArray != null)
				{
					ContentStreamOperators.SetLineDashPatternOperator.Write(writer, context, element.StrokeDashArray, element.StrokeDashOffset);
				}
				if (element.MiterLimit != null)
				{
					ContentStreamOperators.SetMiterLimitOperator.Write(writer, context, element.MiterLimit.Value);
				}
			}
		}
	}
}
