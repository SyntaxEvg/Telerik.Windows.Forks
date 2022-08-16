using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Layout
{
	class PathLayoutElement : GraphicBasedLayoutElementBase<Path>
	{
		public PathLayoutElement(Path path, TextProperties textProperties)
			: base(path, PathLayoutElement.GetSize(path), textProperties)
		{
		}

		internal override Point LayoutBoundsTopLeft
		{
			get
			{
				Rect bounds = base.Element.Geometry.Bounds;
				double num = bounds.X;
				double num2 = bounds.Y;
				if (base.Element.IsStroked)
				{
					double num3 = base.Element.StrokeThickness / 2.0;
					num -= num3;
					num2 -= num3;
				}
				return new Point(num, num2);
			}
		}

		static Size GetSize(Path path)
		{
			Guard.ThrowExceptionIfNull<Path>(path, "path");
			Rect bounds = path.Geometry.Bounds;
			double num = bounds.Width;
			double num2 = bounds.Height;
			if (path.IsStroked)
			{
				num += path.StrokeThickness;
				num2 += path.StrokeThickness;
			}
			return new Size(num, num2);
		}
	}
}
