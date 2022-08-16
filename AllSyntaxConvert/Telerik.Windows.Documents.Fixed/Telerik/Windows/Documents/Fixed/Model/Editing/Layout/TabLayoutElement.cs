using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.Editing.Collections;
using Telerik.Windows.Documents.Fixed.Model.Editing.Flow;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Layout
{
	class TabLayoutElement : LayoutElementBase
	{
		public TabLayoutElement(TabStopCollection tabStopCollection, double defaultTabStopWidth, TextProperties textProperties)
			: base(defaultTabStopWidth, 0.0, TextFragmentLayoutElement.GetBaselineOffset(textProperties.Font, textProperties.FontSize), textProperties.Font, textProperties.FontSize)
		{
			this.tabStopCollection = tabStopCollection;
			this.defaultTabStopWidth = defaultTabStopWidth;
		}

		internal override bool ReflectsOnTabStopWidth
		{
			get
			{
				return false;
			}
		}

		internal override LineInfo CompleteLine(double maxLineWidth, double currentLineOffsetX, List<LayoutElementBase> elementsInCurrentLine, out IEnumerable<LayoutElementBase> pendingLayoutElements)
		{
			LineInfo lineInfo = new LineInfo();
			lineInfo.AddRange(elementsInCurrentLine);
			pendingLayoutElements = new List<LayoutElementBase> { this };
			return lineInfo;
		}

		internal override double GetWidth()
		{
			double num = this.defaultTabStopWidth - this.currentLineOffsetX % this.defaultTabStopWidth;
			if (this.tabStopCollection.Count > 0)
			{
				this.tabStop = (from t in this.tabStopCollection
					where t.Position >= this.currentLineOffsetX
					select t).FirstOrDefault<TabStop>();
				if (this.tabStop != null && this.tabStop.Type != TabStopType.Bar && this.tabStop.Type != TabStopType.Clear)
				{
					num = this.tabStop.Position - this.currentLineOffsetX;
					switch (this.tabStop.Type)
					{
					case TabStopType.Center:
						num = Math.Max(0.0, num - this.tabStopTrailingElementsWidth / 2.0);
						break;
					case TabStopType.Right:
					case TabStopType.Decimal:
						num = Math.Max(0.0, num - this.tabStopTrailingElementsWidth);
						break;
					}
				}
			}
			num = System.Math.Min(num, this.maxTotalWidth);
			this.lastTabWidthCached = num;
			return num;
		}

		internal override void PrepareCalculatingWidthOnMeasure(LineMeasureContext context)
		{
			if (this.ShouldCalculateTrailingWidth())
			{
				this.tabStopTrailingElementsWidth = this.GetTrailingElementsWidth(context, this.tabStop.Type == TabStopType.Decimal);
			}
		}

		internal override void Draw(DrawLayoutElementContext context)
		{
			Guard.ThrowExceptionIfNull<DrawLayoutElementContext>(context, "context");
			if (this.tabStop == null)
			{
				return;
			}
			switch (this.tabStop.Type)
			{
			case TabStopType.Left:
			case TabStopType.Center:
			case TabStopType.Right:
			case TabStopType.Decimal:
				this.DrawTabStopLeader(context);
				return;
			case TabStopType.Bar:
				this.DrawBarTabStop(context);
				break;
			case TabStopType.Clear:
				break;
			default:
				return;
			}
		}

		internal override bool CanFit(double maxTotalWidth, double currentLineOffsetX)
		{
			this.maxTotalWidth = maxTotalWidth;
			this.currentLineOffsetX = currentLineOffsetX;
			return this.currentLineOffsetX + base.Width <= maxTotalWidth;
		}

		bool ShouldCalculateTrailingWidth()
		{
			return this.tabStop != null && (this.tabStop.Type == TabStopType.Center || this.tabStop.Type == TabStopType.Right || this.tabStop.Type == TabStopType.Decimal);
		}

		double GetTrailingElementsWidth(LineMeasureContext context, bool shouldStopOnDecimal)
		{
			double num = 0.0;
			foreach (LayoutElementBase layoutElementBase in context.PendingElements)
			{
				if (!layoutElementBase.ReflectsOnTabStopWidth)
				{
					break;
				}
				if (shouldStopOnDecimal)
				{
					num += layoutElementBase.GetElementWidthFromStartToDecimal();
				}
				else
				{
					num += layoutElementBase.Width;
				}
				if (num >= context.RemainingLineWidth)
				{
					return context.RemainingLineWidth;
				}
			}
			return num;
		}

		void DrawBarTabStop(DrawLayoutElementContext context)
		{
			double x = context.BoundingRectangle.X + this.tabStop.Position;
			double offsetY = context.OffsetY;
			using (context.Editor.SaveGraphicProperties())
			{
				context.Editor.GraphicProperties.StrokeColor = context.Block.GraphicProperties.FillColor;
				context.Editor.DrawLine(new Point(x, offsetY), new Point(x, offsetY + base.Height * 2.0));
			}
		}

		void DrawTabStopLeader(DrawLayoutElementContext context)
		{
			if (this.tabStop.Leader == TabStopLeader.None || this.lastTabWidthCached <= 1.0)
			{
				return;
			}
			double num = context.OffsetX + 1.0;
			double y = context.OffsetY + base.Height - 1.0;
			Point point = default(Point);
			Point point2 = default(Point);
			using (context.Editor.SaveGraphicProperties())
			{
				context.Editor.GraphicProperties.StrokeColor = context.Block.GraphicProperties.FillColor;
				context.Editor.GraphicProperties.IsFilled = false;
				double y2 = context.OffsetY + base.Height / 2.0 + context.Line.Descent;
				switch (this.tabStop.Leader)
				{
				case TabStopLeader.Dot:
					point = new Point(num, y);
					point2 = new Point(this.lastTabWidthCached + num - 1.0, y);
					context.Editor.GraphicProperties.StrokeDashArray = new double[] { 0.5, 2.0 };
					context.Editor.GraphicProperties.StrokeLineCap = LineCap.Round;
					break;
				case TabStopLeader.Hyphen:
					point = new Point(num, y2);
					point2 = new Point(this.lastTabWidthCached + num - 1.0, y2);
					context.Editor.GraphicProperties.StrokeDashArray = new double[] { 3.0, 2.0 };
					break;
				case TabStopLeader.Underscore:
					point = new Point(num, y);
					point2 = new Point(num + this.lastTabWidthCached - 1.0, y);
					context.Editor.GraphicProperties.StrokeDashOffset = 0.0;
					break;
				case TabStopLeader.MiddleDot:
					point = new Point(num, y2);
					point2 = new Point(this.lastTabWidthCached + num - 1.0, y2);
					context.Editor.GraphicProperties.StrokeDashArray = new double[] { 0.5, 2.0 };
					context.Editor.GraphicProperties.StrokeLineCap = LineCap.Round;
					break;
				}
				context.Editor.DrawLine(point, point2);
			}
		}

		const int FontDistance = 1;

		readonly TabStopCollection tabStopCollection;

		readonly double defaultTabStopWidth;

		TabStop tabStop;

		double currentLineOffsetX;

		double tabStopTrailingElementsWidth;

		double maxTotalWidth;

		double lastTabWidthCached;
	}
}
