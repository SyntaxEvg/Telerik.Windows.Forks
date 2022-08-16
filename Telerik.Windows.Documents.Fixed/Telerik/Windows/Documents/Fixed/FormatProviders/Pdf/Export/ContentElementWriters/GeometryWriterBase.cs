using System;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Operators;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.ContentElementWriters
{
	class GeometryWriterBase : ContentElementWriter<GeometryBase>
	{
		protected override void WriteOverride(PdfWriter writer, IPdfContentExportContext context, GeometryBase element)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<GeometryBase>(element, "element");
			PathGeometry pathGeometry = element as PathGeometry;
			if (pathGeometry != null)
			{
				this.WritePathGeometry(writer, context, pathGeometry);
				return;
			}
			RectangleGeometry rectangleGeometry = element as RectangleGeometry;
			if (rectangleGeometry != null)
			{
				this.WriteRectangleGeometry(writer, context, rectangleGeometry);
			}
		}

		protected virtual void WritePathGeometry(PdfWriter writer, IPdfContentExportContext context, PathGeometry pathGeometry)
		{
			foreach (PathFigure pathFigure in pathGeometry.Figures)
			{
				this.WritePathFigure(writer, context, pathFigure);
			}
		}

		protected virtual void WriteRectangleGeometry(PdfWriter writer, IPdfContentExportContext context, RectangleGeometry rectangleGeometry)
		{
			ContentStreamOperators.AppendRectangleOperator.Write(writer, context, rectangleGeometry);
		}

		protected void WritePathFigure(PdfWriter writer, IPdfContentExportContext context, PathFigure pathFigure)
		{
			ContentStreamOperators.MoveToOperator.Write(writer, context, pathFigure.StartPoint);
			Point previousPoint = pathFigure.StartPoint;
			foreach (PathSegment pathSegment in pathFigure.Segments)
			{
				ArcSegment arcSegment = pathSegment as ArcSegment;
				QuadraticBezierSegment quadraticBezierSegment = pathSegment as QuadraticBezierSegment;
				if (arcSegment != null)
				{
					using (IEnumerator<BezierSegment> enumerator2 = arcSegment.ToBezierSegments(previousPoint).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							BezierSegment element = enumerator2.Current;
							ContentElementWriterBase.WriteElement(writer, context, element);
						}
						goto IL_99;
					}
					goto IL_7A;
				}
				goto IL_7A;
				IL_99:
				previousPoint = pathSegment.LastPoint;
				continue;
				IL_7A:
				if (quadraticBezierSegment != null)
				{
					BezierSegment element2 = quadraticBezierSegment.ToBezierSegment(previousPoint);
					ContentElementWriterBase.WriteElement(writer, context, element2);
					goto IL_99;
				}
				ContentElementWriterBase.WriteElement(writer, context, pathSegment);
				goto IL_99;
			}
			if (pathFigure.IsClosed)
			{
				ContentStreamOperators.ClosePathOperator.Write(writer, context);
			}
		}
	}
}
