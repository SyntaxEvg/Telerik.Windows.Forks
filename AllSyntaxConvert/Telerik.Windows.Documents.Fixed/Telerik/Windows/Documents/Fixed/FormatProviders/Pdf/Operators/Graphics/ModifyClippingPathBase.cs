using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Graphics
{
	abstract class ModifyClippingPathBase : ContentStreamOperator
	{
		protected abstract FillRule FillRule { get; }

		public override void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
			Guard.ThrowExceptionIfNull<ContentStreamInterpreter>(interpreter, "interpreter");
			Guard.ThrowExceptionIfNull<IPdfContentImportContext>(context, "context");
			Clipping clipping = new Clipping();
			clipping.Clip = interpreter.CalculateGeometry();
			clipping.Transform = context.ContentRoot.ToTopLeftCoordinateSystem(interpreter.GraphicState.Matrix);
			PathGeometry pathGeometry = clipping.Clip as PathGeometry;
			if (pathGeometry != null)
			{
				pathGeometry.FillRule = this.FillRule;
			}
			interpreter.GraphicState.IntersectClipping(clipping);
		}

		public void Write(PdfWriter writer, IPdfContentExportContext context)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			base.WriteInternal(writer, context.Owner, new PdfPrimitive[0]);
		}
	}
}
