using System;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Color
{
	abstract class SetSimpleColorBase<TColor, TColorSpace> : ContentStreamOperator where TColor : ColorObjectBase where TColorSpace : ColorSpaceObject
	{
		protected abstract bool IsStrokeColorSetter { get; }

		public sealed override void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
			Guard.ThrowExceptionIfNull<ContentStreamInterpreter>(interpreter, "interpreter");
			Guard.ThrowExceptionIfNull<IPdfContentImportContext>(context, "context");
			TColor tcolor;
			TColorSpace tcolorSpace;
			this.ReadColorComponents(interpreter, context, out tcolor, out tcolorSpace);
			if (this.IsStrokeColorSetter)
			{
				interpreter.GraphicState.SetStrokeColorSpace(tcolorSpace);
				interpreter.GraphicState.StrokeColor = tcolor;
				return;
			}
			interpreter.GraphicState.SetFillColorSpace(tcolorSpace);
			interpreter.GraphicState.FillColor = tcolor;
		}

		protected abstract void ReadColorComponents(ContentStreamInterpreter interpreter, IPdfContentImportContext context, out TColor color, out TColorSpace colorspace);

		protected static PdfReal ReadRestrictedNumberComponentValue(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
			PdfReal last = interpreter.Operands.GetLast<PdfReal>(interpreter.Reader, context.Owner);
			double initialValue = global::Telerik.Windows.Documents.Core.Imaging.Color.RestrictComponentToDoubleLimits(last.Value);
			return new PdfReal(initialValue);
		}
	}
}
