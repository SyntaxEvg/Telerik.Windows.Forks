using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Graphics
{
	abstract class PaintPathBase : ContentStreamOperator
	{
		public override void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
			Guard.ThrowExceptionIfNull<ContentStreamInterpreter>(interpreter, "interpreter");
			Guard.ThrowExceptionIfNull<IPdfContentImportContext>(context, "context");
			Path path = context.ContentRoot.Content.AddPath();
			path.Geometry = interpreter.CalculateGeometry();
			interpreter.ClearGeometry();
			path.Position = new MatrixPosition(context.ContentRoot.ToTopLeftCoordinateSystem(interpreter.GraphicState.Matrix));
			path.Clipping = interpreter.GraphicState.Clipping;
			path.Marker = context.CurrentMarker;
			this.ExecuteOverride(interpreter, context, path);
			interpreter.ApplyGeometryProperties(path.GeometryProperties);
		}

		public void Write(PdfWriter writer, IPdfContentExportContext context)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			base.WriteInternal(writer, context.Owner, new PdfPrimitive[0]);
		}

		protected abstract void ExecuteOverride(ContentStreamInterpreter interpreter, IPdfContentImportContext context, Path path);
	}
}
