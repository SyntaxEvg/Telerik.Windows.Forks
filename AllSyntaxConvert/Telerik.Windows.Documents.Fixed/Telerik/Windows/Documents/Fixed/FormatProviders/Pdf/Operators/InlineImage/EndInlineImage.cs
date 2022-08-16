using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Objects;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Objects;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.InlineImage
{
	class EndInlineImage : ContentStreamOperator
	{
		public override string Name
		{
			get
			{
				return "EI";
			}
		}

		public override void Execute(ContentStreamInterpreter contentStreamInterpreter, IPdfContentImportContext context)
		{
			InlineImagePdfStream lastAs = contentStreamInterpreter.Operands.GetLastAs<InlineImagePdfStream>();
			ImageXObject imageXObject = new ImageXObject();
			imageXObject.Load(contentStreamInterpreter.Reader, context.Owner, lastAs);
			Paint.PaintImage(contentStreamInterpreter, context, imageXObject);
		}
	}
}
