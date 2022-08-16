using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators;
using Telerik.Windows.Documents.Fixed.Model;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.ContentElementWriters
{
	class TextPropertiesWriter : ContentElementWriter<TextPropertiesOwner>
	{
		protected override void WriteOverride(PdfWriter writer, IPdfContentExportContext context, TextPropertiesOwner element)
		{
			ContentStreamOperators.TextMatrixOperator.Write(writer, context, element.TextMatrix);
			if (element.HorizontalScaling != null)
			{
				ContentStreamOperators.HorizontalScaleOperator.Write(writer, context, element.HorizontalScaling.Value);
			}
			if (element.CharacterSpacing != null)
			{
				ContentStreamOperators.CharSpaceOperator.Write(writer, context, element.CharacterSpacing.Value);
			}
			if (element.WordSpacing != null)
			{
				ContentStreamOperators.WordSpaceOperator.Write(writer, context, element.WordSpacing.Value);
			}
			if (element.TextRise != null)
			{
				ContentStreamOperators.RiseOperator.Write(writer, context, element.TextRise.Value);
			}
			ContentStreamOperators.RenderingModeOperator.Write(writer, context, element.RenderingMode);
			ContentElementWriters.GeometryPropertiesWriter.Write(writer, context, element.GeometryProperties);
			ContentStreamOperators.FontOperator.Write(writer, context, context.GetResource(element.Font).ResourceKey, element.FontSize);
		}
	}
}
