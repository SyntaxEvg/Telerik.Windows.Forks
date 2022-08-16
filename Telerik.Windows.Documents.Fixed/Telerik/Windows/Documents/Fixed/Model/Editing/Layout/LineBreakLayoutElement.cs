using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Fixed.Model.Fonts;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Layout
{
	class LineBreakLayoutElement : LayoutElementBase
	{
		public LineBreakLayoutElement(FontBase font, double fontSize)
			: base(0.0, 0.0, TextFragmentLayoutElement.GetBaselineOffset(font, fontSize), font, fontSize)
		{
		}

		internal override bool IsLineBreak
		{
			get
			{
				return true;
			}
		}

		internal override bool ReflectsOnTabStopWidth
		{
			get
			{
				return false;
			}
		}

		internal override void Draw(DrawLayoutElementContext context)
		{
		}

		internal override bool CanFit(double actualWidth, double offsetX)
		{
			return false;
		}

		internal override LineInfo CompleteLine(double maxLineWidth, double currentLineOffsetX, List<LayoutElementBase> elementsInCurrentLine, out IEnumerable<LayoutElementBase> pendingLayoutElements)
		{
			LineInfo lineInfo = new LineInfo();
			lineInfo.AddRange(elementsInCurrentLine);
			lineInfo.Add(this);
			pendingLayoutElements = Enumerable.Empty<LayoutElementBase>();
			return lineInfo;
		}
	}
}
