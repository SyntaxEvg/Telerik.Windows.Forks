using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.Collections;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Extensions
{
	static class AnnotationExtentions
	{
		internal static bool TryGetAnnotationFromLocation(this AnnotationCollection annotations, Point location, out Annotation annotation)
		{
			annotation = null;
			bool result = false;
			for (int i = annotations.Count - 1; i >= 0; i--)
			{
				Annotation annotation2 = annotations[i];
				Guard.ThrowExceptionIfNull<Rect?>(annotation2.BoundingRect, "annot");
				if (annotation2.BoundingRect.Value.Contains(location))
				{
					annotation = annotation2;
					result = true;
					break;
				}
			}
			return result;
		}
	}
}
