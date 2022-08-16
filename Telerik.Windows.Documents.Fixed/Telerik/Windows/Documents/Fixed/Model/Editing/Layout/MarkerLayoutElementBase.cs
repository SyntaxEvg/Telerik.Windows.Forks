using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.Model.Fonts;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Layout
{
	class MarkerLayoutElementBase : LayoutElementBase
	{
		public MarkerLayoutElementBase(FontBase font, double fontSize)
			: base(0.0, 0.0, 0.0, font, fontSize)
		{
		}

		internal override void Draw(DrawLayoutElementContext context)
		{
		}

		internal override bool CanFit(double actualWidth, double offsetX)
		{
			return true;
		}

		internal override LineInfo CompleteLine(double maxLineWidth, double currentLineOffsetX, List<LayoutElementBase> elementsInCurrentLine, out IEnumerable<LayoutElementBase> pendingLayoutElements)
		{
			throw new NotSupportedException();
		}
	}
}
