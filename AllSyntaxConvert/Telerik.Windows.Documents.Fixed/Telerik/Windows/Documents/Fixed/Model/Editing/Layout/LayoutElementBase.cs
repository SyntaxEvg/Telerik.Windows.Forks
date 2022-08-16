using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Layout
{
	public abstract class LayoutElementBase
	{
		internal LayoutElementBase(double width, double trimmedWidth, double baselineOffset, FontBase font, double fontSize)
		{
			Guard.ThrowExceptionIfNull<FontBase>(font, "font");
			this.width = width;
			this.trimmedWidth = trimmedWidth;
			this.font = font;
			this.fontSize = fontSize;
			this.baselineOffset = baselineOffset;
			this.lineSpacingHeight = TextFragmentLayoutElement.GetBaselineOffset(this.font, this.fontSize);
			this.descent = Math.Abs(this.font.Descent.Value) * this.FontSize;
			this.linespacingDescent = this.descent;
		}

		public double Width
		{
			get
			{
				return this.GetWidth();
			}
		}

		internal virtual double GetWidth()
		{
			return this.width;
		}

		public double Height
		{
			get
			{
				return this.BaselineOffset;
			}
		}

		internal double BaselineOffset
		{
			get
			{
				return this.baselineOffset;
			}
		}

		internal double TrimmedWidth
		{
			get
			{
				return this.trimmedWidth;
			}
		}

		internal double FontSize
		{
			get
			{
				return this.fontSize;
			}
		}

		internal FontBase Font
		{
			get
			{
				return this.font;
			}
		}

		internal virtual bool IsLineBreak
		{
			get
			{
				return false;
			}
		}

		internal virtual bool ReflectsOnTabStopWidth
		{
			get
			{
				return true;
			}
		}

		internal virtual double Descent
		{
			get
			{
				return this.descent;
			}
		}

		internal double UnderlinePosition
		{
			get
			{
				return Math.Abs(this.Font.UnderlinePosition) * this.FontSize;
			}
		}

		internal double UnderlineThickness
		{
			get
			{
				return this.Font.UnderlineThickness * this.FontSize;
			}
		}

		internal double LineSpacingBaselineOffset
		{
			get
			{
				return this.lineSpacingHeight;
			}
		}

		internal virtual double LineSpacingDescent
		{
			get
			{
				return this.linespacingDescent;
			}
		}

		internal virtual bool CanAddToLineEnding
		{
			get
			{
				return false;
			}
		}

		internal virtual bool CanMergeWithNextLayoutElement
		{
			get
			{
				return false;
			}
		}

		internal virtual bool CanSplitToMergeWithNextLayoutElement
		{
			get
			{
				return false;
			}
		}

		internal abstract LineInfo CompleteLine(double maxLineWidth, double currentLineOffsetX, List<LayoutElementBase> elementsInCurrentLine, out IEnumerable<LayoutElementBase> pendingLayoutElements);

		internal virtual void SplitToMergeWithNextLayoutElement(out LayoutElementBase firstPart, out LayoutElementBase secondPart)
		{
			firstPart = null;
			secondPart = null;
		}

		internal virtual bool TrySplitToAddToLineEnding(out LayoutElementBase firstPart, out LayoutElementBase secondPart)
		{
			firstPart = null;
			secondPart = null;
			return false;
		}

		internal virtual bool ShouldSplitToBlocks(Block block)
		{
			return false;
		}

		internal virtual void SplitToBlocks(Block block, out LayoutElementBase element, out IEnumerable<LayoutElementBase> elementsToInclude, out IEnumerable<LayoutElementBase> pendingElements)
		{
			throw new NotSupportedException();
		}

		internal double GetWidthInLine(LineInfo line)
		{
			Guard.ThrowExceptionIfNull<LineInfo>(line, "line");
			if (line.Elements.Last<LayoutElementBase>() != this)
			{
				return this.Width;
			}
			return this.TrimmedWidth;
		}

		internal abstract void Draw(DrawLayoutElementContext context);

		internal abstract bool CanFit(double maxTotalWidth, double currentLineOffsetX);

		internal virtual void PrepareCalculatingWidthOnMeasure(LineMeasureContext context)
		{
		}

		internal virtual double GetElementWidthFromStartToDecimal()
		{
			return this.Width;
		}

		internal virtual double GetMinWidth()
		{
			return this.Width;
		}

		internal virtual Size GetMinMeasureSize()
		{
			return new Size(this.Width, this.Height);
		}

		readonly FontBase font;

		readonly double fontSize;

		readonly double width;

		readonly double trimmedWidth;

		readonly double baselineOffset;

		readonly double descent;

		readonly double lineSpacingHeight;

		readonly double linespacingDescent;
	}
}
