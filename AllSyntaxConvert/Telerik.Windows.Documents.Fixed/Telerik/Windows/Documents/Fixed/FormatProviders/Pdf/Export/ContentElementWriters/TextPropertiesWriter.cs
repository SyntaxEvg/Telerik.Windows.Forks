using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators;
using Telerik.Windows.Documents.Fixed.Model;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.ContentElementWriters
{
	class TextPropertiesWriter : global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.ContentElementWriters.ContentElementWriter<global::Telerik.Windows.Documents.Fixed.Model.TextPropertiesOwner>
	{
		protected override void WriteOverride(global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.PdfWriter writer, global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.IPdfContentExportContext context, global::Telerik.Windows.Documents.Fixed.Model.TextPropertiesOwner element)
		{
			global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.ContentStreamOperators.TextMatrixOperator.Write(writer, context, element.TextMatrix);
			if (element.HorizontalScaling != null)
			{
				global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.ContentStreamOperators.HorizontalScaleOperator.Write(writer, context, element.HorizontalScaling.Value);
			}
			if (element.CharacterSpacing != null)
			{
				global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.ContentStreamOperators.CharSpaceOperator.Write(writer, context, element.CharacterSpacing.Value);
			}
			if (element.WordSpacing != null)
			{
				global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.ContentStreamOperators.WordSpaceOperator.Write(writer, context, element.WordSpacing.Value);
			}
			if (element.TextRise != null)
			{
				global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.ContentStreamOperators.RiseOperator.Write(writer, context, element.TextRise.Value);
			}
			global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.ContentStreamOperators.RenderingModeOperator.Write(writer, context, element.RenderingMode);
			global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.ContentElementWriters.ContentElementWriters.GeometryPropertiesWriter.Write(writer, context, element.GeometryProperties);
			global::Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.ContentStreamOperators.FontOperator.Write(writer, context, context.GetResource(element.Font).ResourceKey, element.FontSize);
		}
	}
}
