using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Layout
{
	class SpaceLayoutElement : LayoutElementBase
	{
		public SpaceLayoutElement(TextProperties textProperties)
			: this(new Size(0.0, TextFragmentLayoutElement.GetBaselineOffset(textProperties.Font, textProperties.FontSize)), textProperties)
		{
		}

		public SpaceLayoutElement(Size size, TextProperties textProperties)
			: base(size.Width, 0.0, size.Height, textProperties.Font, textProperties.FontSize)
		{
		}

		internal override LineInfo CompleteLine(double maxLineWidth, double currentLineOffsetX, List<LayoutElementBase> elementsInCurrentLine, out IEnumerable<LayoutElementBase> pendingLayoutElements)
		{
			LineInfo lineInfo = new LineInfo();
			lineInfo.AddRange(elementsInCurrentLine);
			lineInfo.Add(this);
			pendingLayoutElements = Enumerable.Empty<LayoutElementBase>();
			return lineInfo;
		}

		internal override void Draw(DrawLayoutElementContext context)
		{
		}

		internal override bool CanFit(double maxTotalWidth, double currentLineOffsetX)
		{
			return true;
		}
	}
}
