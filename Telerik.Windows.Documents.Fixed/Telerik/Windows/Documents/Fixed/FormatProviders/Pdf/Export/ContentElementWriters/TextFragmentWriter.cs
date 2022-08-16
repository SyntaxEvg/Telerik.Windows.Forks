using System;
using System.Linq;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Text;
using Telerik.Windows.Documents.Fixed.Model.Text;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.ContentElementWriters
{
	class TextFragmentWriter : MarkableContentElementWriter<TextFragment>
	{
		protected override void WriteOverride(PdfWriter writer, IPdfContentExportContext context, TextFragment element)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<TextFragment>(element, "element");
			if (element.TextCollection.Characters.Any<CharInfo>())
			{
				using (ContentStreamOperators.PushGraphicsState(writer, context))
				{
					Matrix matrix = element.TextMatrix.InverseMatrix().MultiplyBy(ShowText.FlipTextTransformation).MultiplyBy(element.Position.Matrix);
					ContentStreamOperators.ConcatMatrixOperator.Write(writer, context, matrix);
					ContentStreamOperators.BeginTextOperator.Write(writer, context);
					ContentElementWriters.TextPropertiesWriter.Write(writer, context, element.TextProperties);
					ContentStreamOperators.ShowTextOperator.Write(writer, context, element.TextCollection);
					context.SetUsedCharacters(element.Font, element.TextCollection);
					ContentStreamOperators.EndTextOperator.Write(writer, context);
				}
			}
		}
	}
}
