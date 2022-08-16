using System;
using System.Collections.Generic;
using System.Linq;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Layout
{
	class BulletLayoutElement : LayoutElementBase
	{
		public BulletLayoutElement(LineInfo bulletElementsLine, double indentAfterBullet, TextProperties textProperties)
			: base(bulletElementsLine.Width + indentAfterBullet, bulletElementsLine.TrimmedWidth, bulletElementsLine.BaselineOffset, textProperties.Font, textProperties.FontSize)
		{
			this.bulletElements = bulletElementsLine.Elements;
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
			Block.Draw(this.bulletElements, context);
		}

		internal override bool CanFit(double maxTotalWidth, double currentLineOffsetX)
		{
			return true;
		}

		public override string ToString()
		{
			return this.bulletElements.ToString();
		}

		readonly IEnumerable<LayoutElementBase> bulletElements;
	}
}
