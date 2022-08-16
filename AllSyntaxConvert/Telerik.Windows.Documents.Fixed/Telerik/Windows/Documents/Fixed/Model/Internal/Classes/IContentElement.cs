using System;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Core.Shapes;

namespace Telerik.Windows.Documents.Fixed.Model.Internal.Classes
{
	interface IContentElement
	{
		int ZIndex { get; set; }

		PathGeometry Clip { get; set; }

		bool HasChildren { get; }

		Matrix TransformMatrix { get; set; }

		Size Size { get; set; }

		Rect BoundingRect { get; set; }

		ContentElementTypeOld Type { get; }

		IEnumerable<IContentElement> Children { get; }

		Rect Arrange(Matrix transformMatrix);

		IContentElement Clone();
	}
}
