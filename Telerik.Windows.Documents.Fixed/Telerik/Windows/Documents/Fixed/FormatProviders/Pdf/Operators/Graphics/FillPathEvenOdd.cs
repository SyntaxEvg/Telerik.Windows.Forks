using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Graphics
{
	class FillPathEvenOdd : PaintPathBase
	{
		public override string Name
		{
			get
			{
				return "f*";
			}
		}

		protected override void ExecuteOverride(ContentStreamInterpreter interpreter, IPdfContentImportContext context, Path path)
		{
			Guard.ThrowExceptionIfNull<ContentStreamInterpreter>(interpreter, "interpreter");
			Guard.ThrowExceptionIfNull<IPdfContentImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Path>(path, "path");
			path.IsFilled = true;
			path.IsStroked = false;
			PathGeometry pathGeometry = path.Geometry as PathGeometry;
			if (pathGeometry != null)
			{
				pathGeometry.FillRule = FillRule.EvenOdd;
			}
		}
	}
}
