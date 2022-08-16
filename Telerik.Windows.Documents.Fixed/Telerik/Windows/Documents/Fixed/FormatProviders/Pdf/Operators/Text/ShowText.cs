using System;
using System.Collections.Generic;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Fixed.Model.Editing.Layout;
using Telerik.Windows.Documents.Fixed.Model.Text;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Text
{
	class ShowText : ContentStreamOperator
	{
		public override string Name
		{
			get
			{
				return "Tj";
			}
		}

		public override void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
			Guard.ThrowExceptionIfNull<ContentStreamInterpreter>(interpreter, "interpreter");
			Guard.ThrowExceptionIfNull<IPdfContentImportContext>(context, "context");
			if (interpreter.Operands.Count > 0)
			{
				PdfString last = interpreter.Operands.GetLast<PdfString>(interpreter.Reader, context.Owner);
				ShowText.Execute(interpreter, context, last);
			}
		}

		public static void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context, PdfString text)
		{
			if (interpreter.TextState.Font != null)
			{
				TextFragment textFragment = context.ContentRoot.Content.AddTextFragment();
				interpreter.ApplyTextProperties(textFragment.TextProperties);
				textFragment.TextCollection.Characters = interpreter.TextState.Font.GetCharacters(text);
				Matrix matrix = ShowText.FlipTextTransformation.MultiplyBy(interpreter.TextRenderingMatrix);
				textFragment.Position = new MatrixPosition(context.ContentRoot.ToTopLeftCoordinateSystem(matrix));
				textFragment.Clipping = interpreter.GraphicState.Clipping;
				textFragment.Marker = context.CurrentMarker;
				double tx = TextFragmentLayoutElement.MeasureWidth(textFragment);
				TranslateText.MoveHorizontalPosition(interpreter.TextState, tx);
			}
		}

		public void Write(PdfWriter writer, IPdfContentExportContext context, TextCollection glyphs)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			List<byte> list = new List<byte>();
			foreach (CharInfo charInfo in glyphs.Characters)
			{
				list.AddRange(charInfo.CharCode.ToBytes());
			}
			PdfString pdfString = new PdfHexString(list.ToArray());
			base.WriteInternal(writer, context.Owner, new PdfPrimitive[] { pdfString });
		}

		public static readonly Matrix FlipTextTransformation = new Matrix(1.0, 0.0, 0.0, -1.0, 0.0, 0.0);
	}
}
