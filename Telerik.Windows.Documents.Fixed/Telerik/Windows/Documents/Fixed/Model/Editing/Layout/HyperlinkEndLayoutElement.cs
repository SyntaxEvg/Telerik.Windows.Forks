using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Layout
{
	class HyperlinkEndLayoutElement : EndMarkerLayoutElement<HyperlinkStartLayoutElement>
	{
		public HyperlinkEndLayoutElement(HyperlinkStartLayoutElement start, FontBase font, double fontSize)
			: base(start, font, fontSize)
		{
		}

		internal override void Draw(DrawLayoutElementContext context)
		{
			Guard.ThrowExceptionIfNull<DrawLayoutElementContext>(context, "context");
			Guard.ThrowExceptionIfNotEqual<Block>(base.Start.Block, context.Block, "block");
			if (context.Editor.SupportsAnnotations)
			{
				Block block = context.Block;
				LineInfo line = base.Start.Line;
				LineInfo line2 = context.Line;
				HyperlinkStartLayoutElement start = base.Start;
				foreach (LineInfo line3 in block.GetLinesBetween(line, line2))
				{
					context.Editor.AddAnnotation(HyperlinkEndLayoutElement.CreateAnnotation(line3, start));
				}
				if (line == line2)
				{
					context.Editor.AddAnnotation(HyperlinkEndLayoutElement.CreateAnnotation(line, start, base.Start.OffsetX, context.OffsetX));
					return;
				}
				context.Editor.AddAnnotation(HyperlinkEndLayoutElement.CreateAnnotation(line, start, base.Start.OffsetX, line.BoundingRect.X + line.BoundingRect.Width));
				context.Editor.AddAnnotation(HyperlinkEndLayoutElement.CreateAnnotation(line2, start, 0.0, context.OffsetX));
			}
		}

		static Annotation CreateAnnotation(LineInfo line, HyperlinkStartLayoutElement hyperlinkStart)
		{
			Guard.ThrowExceptionIfNull<LineInfo>(line, "line");
			Guard.ThrowExceptionIfNull<HyperlinkStartLayoutElement>(hyperlinkStart, "hyperlinkStart");
			return HyperlinkEndLayoutElement.CreateAnnotation(line.BoundingRect, hyperlinkStart);
		}

		static Annotation CreateAnnotation(LineInfo line, HyperlinkStartLayoutElement hyperlinkStart, double x1, double x2)
		{
			Guard.ThrowExceptionIfNull<LineInfo>(line, "line");
			Guard.ThrowExceptionIfNull<HyperlinkStartLayoutElement>(hyperlinkStart, "hyperlinkStart");
			double width = Math.Max(0.0, x2 - x1);
			return HyperlinkEndLayoutElement.CreateAnnotation(new Rect(x1, line.BoundingRect.Y, width, line.BoundingRect.Height), hyperlinkStart);
		}

		static Annotation CreateAnnotation(Rect rect, HyperlinkStartLayoutElement hyperlinkStart)
		{
			Guard.ThrowExceptionIfNull<HyperlinkStartLayoutElement>(hyperlinkStart, "hyperlinkStart");
			Annotation annotation = hyperlinkStart.CreateAnnotation();
			annotation.Rect = rect;
			return annotation;
		}
	}
}
