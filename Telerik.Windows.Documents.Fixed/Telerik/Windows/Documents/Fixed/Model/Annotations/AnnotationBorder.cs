using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Fixed.Model.Annotations
{
	public class AnnotationBorder
	{
		internal static AnnotationBorder DefaultAnnotationBorder
		{
			get
			{
				return new AnnotationBorder(0.0, AnnotationBorderStyle.None, null);
			}
		}

		public AnnotationBorder()
		{
		}

		public AnnotationBorder(double width, AnnotationBorderStyle style, double[] dashArray)
		{
			this.Width = width;
			this.Style = style;
			this.DashArray = dashArray;
		}

		public double Width { get; set; }

		public AnnotationBorderStyle Style { get; set; }

		public IEnumerable<double> DashArray { get; set; }
	}
}
