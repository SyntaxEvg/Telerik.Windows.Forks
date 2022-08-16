using System;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Editing.Flow;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Layout
{
	class GraphicBasedLayoutElementBase<T> : ContentElementLayoutElementBase<T> where T : PositionContentElement
	{
		internal GraphicBasedLayoutElementBase(T element, Size size, TextProperties properties)
			: base(element, size.Width, size.Width, size.Height, properties.Font, properties.FontSize, properties.RenderingMode, properties.UnderlineColor, properties.UnderlinePattern, properties.HighlightColor)
		{
		}

		internal override double Descent
		{
			get
			{
				if (base.UnderlinePattern != UnderlinePattern.None)
				{
					return base.Descent;
				}
				return 0.0;
			}
		}

		internal virtual Point LayoutBoundsTopLeft
		{
			get
			{
				return default(Point);
			}
		}

		protected override Matrix Transform(DrawLayoutElementContext context, Matrix transform)
		{
			Guard.ThrowExceptionIfNull<DrawLayoutElementContext>(context, "context");
			Point layoutBoundsTopLeft = this.LayoutBoundsTopLeft;
			double deltaX = -layoutBoundsTopLeft.X;
			double deltaY = context.GetLineActualBaselineOffset() - base.BaselineOffset - layoutBoundsTopLeft.Y;
			return base.Transform(context, transform).TranslateMatrix(deltaX, deltaY);
		}
	}
}
