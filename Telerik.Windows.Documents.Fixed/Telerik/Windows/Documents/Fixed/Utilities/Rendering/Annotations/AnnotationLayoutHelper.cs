using System;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Utilities.Rendering.Annotations
{
	class AnnotationLayoutHelper
	{
		public AnnotationLayoutHelper(Annotation annotation)
		{
			this.annotation = annotation;
		}

		internal Rect CalculateAnnotationBoundingRectangle()
		{
			Matrix matrix = PageLayoutHelper.CalculateVisibibleContentTransformation((RadFixedPage)this.annotation.Parent);
			return matrix.Transform(this.annotation.Rect);
		}

		readonly Annotation annotation;
	}
}
