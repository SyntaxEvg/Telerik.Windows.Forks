using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Fixed.Model.Objects;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.ContentElementWriters
{
	class FormWriter : MarkableContentElementWriter<Form>
	{
		protected override void WriteOverride(PdfWriter writer, IPdfContentExportContext context, Form element)
		{
			using (ContentStreamOperators.PushGraphicsState(writer, context))
			{
				bool applyTopLeftTransformation = true;
				Matrix scaledPosition = element.GetScaledPosition(applyTopLeftTransformation);
				MatrixPosition position = new MatrixPosition(scaledPosition);
				ResourceEntry resource = context.GetResource(element.FormSource);
				ContentStreamOperators.ConcatMatrixOperator.Write(writer, context, position);
				ContentStreamOperators.PaintOperator.Write(writer, context, resource.ResourceKey);
			}
		}
	}
}
