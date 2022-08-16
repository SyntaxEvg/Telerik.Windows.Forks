using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Converters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Text
{
	class ShowTextArray : ContentStreamOperator
	{
		public override string Name
		{
			get
			{
				return "TJ";
			}
		}

		IConverter RealNumberConverter
		{
			get
			{
				if (this.realNumberConverter == null)
				{
					this.realNumberConverter = PdfObjectDescriptors.GetPdfObjectDescriptor<PdfReal>().Converter;
				}
				return this.realNumberConverter;
			}
		}

		public override void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
			PdfArray last = interpreter.Operands.GetLast<PdfArray>(interpreter.Reader, context.Owner);
			foreach (PdfPrimitive pdfPrimitive in last)
			{
				PdfString pdfString = pdfPrimitive as PdfString;
				if (pdfString == null)
				{
					PdfReal pdfReal = (PdfReal)this.RealNumberConverter.Convert(typeof(PdfReal), interpreter.Reader, context.Owner, pdfPrimitive);
					double? horizontalScaling = interpreter.TextState.HorizontalScaling;
					double num = ((horizontalScaling != null) ? horizontalScaling.GetValueOrDefault() : 1.0);
					double tx = -pdfReal.Value / 1000.0 * interpreter.TextState.FontSize * num;
					TranslateText.MoveHorizontalPosition(interpreter.TextState, tx);
				}
				else
				{
					ShowText.Execute(interpreter, context, pdfString);
				}
			}
		}

		IConverter realNumberConverter;
	}
}
