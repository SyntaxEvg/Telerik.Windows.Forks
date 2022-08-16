using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Graphics
{
	class FillAndStrokePathEvenOdd : StrokePathBase
	{
		public override string Name
		{
			get
			{
				return "B*";
			}
		}

		protected override void ExecuteStrokeOverride(ContentStreamInterpreter interpreter, IPdfContentImportContext context, Path path)
		{
			Guard.ThrowExceptionIfNull<ContentStreamInterpreter>(interpreter, "interpreter");
			Guard.ThrowExceptionIfNull<IPdfContentImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Path>(path, "path");
			path.Fill = interpreter.GraphicState.CalculateFillColor(interpreter.Reader, context);
			path.IsFilled = true;
			path.IsStroked = true;
			PathGeometry pathGeometry = path.Geometry as PathGeometry;
			if (pathGeometry != null)
			{
				pathGeometry.FillRule = FillRule.EvenOdd;
			}
		}
	}
}
