using System;
using System.Linq;
using System.Windows;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators.Graphics
{
	class MoveTo : ContentStreamOperator
	{
		public override string Name
		{
			get
			{
				return "m";
			}
		}

		public override void Execute(ContentStreamInterpreter interpreter, IPdfContentImportContext context)
		{
			Guard.ThrowExceptionIfNull<ContentStreamInterpreter>(interpreter, "interpreter");
			Guard.ThrowExceptionIfNull<IPdfContentImportContext>(context, "context");
			PdfReal last = interpreter.Operands.GetLast<PdfReal>(interpreter.Reader, context.Owner);
			PdfReal last2 = interpreter.Operands.GetLast<PdfReal>(interpreter.Reader, context.Owner);
			MoveTo.Execute(interpreter, last2, last);
		}

		public static void Execute(ContentStreamInterpreter interpreter, PdfReal x, PdfReal y)
		{
			PathFigure pathFigure = interpreter.PathFigures.LastOrDefault<PathFigure>();
			if (pathFigure != null && pathFigure.Segments.Count == 0)
			{
				interpreter.PathFigures.Remove(pathFigure);
			}
			interpreter.CurrentPathFigure = interpreter.PathFigures.AddPathFigure();
			interpreter.CurrentPathFigure.StartPoint = new Point(x.Value, y.Value);
		}

		public void Write(PdfWriter writer, IPdfContentExportContext context, Point point)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Point>(point, "point");
			base.WriteInternal(writer, context.Owner, new PdfPrimitive[]
			{
				point.X.ToPdfReal(),
				point.Y.ToPdfReal()
			});
		}
	}
}
