using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Core.Shapes;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.Model.Internal.Classes;

namespace Telerik.Windows.Documents.Fixed.Model.Internal
{
	class Path : Telerik.Windows.Documents.Fixed.Model.Internal.Classes.ContentElement
	{
		public PathGeometry Data { get; set; }

		public Brush Fill { get; set; }

		public double StrokeThickness { get; set; }

		public PenLineCap StrokeStartLineCap { get; set; }

		public PenLineCap StrokeEndLineCap { get; set; }

		public PenLineJoin StrokeLineJoin { get; set; }

		public double StrokeMiterLimit { get; set; }

		public double StrokeDashOffset { get; set; }

		public IEnumerable<double> StrokeDashArray { get; set; }

		public Brush Stroke { get; set; }

		public override ContentElementTypeOld Type
		{
			get
			{
				return ContentElementTypeOld.Path;
			}
		}

		public override IContentElement Clone()
		{
			Path path = new Path();
			path.Fill = this.Fill;
			path.Size = base.Size;
			path.Stroke = this.Stroke;
			path.StrokeDashOffset = this.StrokeDashOffset;
			path.StrokeEndLineCap = this.StrokeEndLineCap;
			path.StrokeLineJoin = this.StrokeLineJoin;
			path.StrokeMiterLimit = this.StrokeMiterLimit;
			path.StrokeStartLineCap = this.StrokeStartLineCap;
			path.StrokeThickness = this.StrokeThickness;
			path.TransformMatrix = base.TransformMatrix;
			path.ZIndex = base.ZIndex;
			if (this.Data != null)
			{
				path.Data = this.Data.Clone();
			}
			if (this.StrokeDashArray != null)
			{
				path.StrokeDashArray = (double[])this.StrokeDashArray.ToArray<double>().Clone();
			}
			return path;
		}

		public override Rect Arrange(Matrix transformMatrix)
		{
			base.BoundingRect = Helper.GetBoundingRect(this.Data.GetBoundingRect(), base.TransformMatrix * transformMatrix);
			return base.BoundingRect;
		}
	}
}
