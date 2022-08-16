using System;
using System.Windows;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Layout
{
	class DrawLayoutElementContext
	{
		public DrawLayoutElementContext(FixedContentEditor editor, Block block, LineInfo line, double offsetX, double offsetY, Rect boundingRectangle)
		{
			Guard.ThrowExceptionIfNull<FixedContentEditor>(editor, "editor");
			Guard.ThrowExceptionIfNull<Block>(block, "block");
			Guard.ThrowExceptionIfNull<LineInfo>(line, "line");
			this.block = block;
			this.editor = editor;
			this.line = line;
			this.offsetX = offsetX;
			this.offsetY = offsetY;
			this.boundingRectangle = boundingRectangle;
		}

		public Rect BoundingRectangle
		{
			get
			{
				return this.boundingRectangle;
			}
		}

		public double OffsetY
		{
			get
			{
				return this.offsetY;
			}
		}

		public double OffsetX
		{
			get
			{
				return this.offsetX;
			}
		}

		public LineInfo Line
		{
			get
			{
				return this.line;
			}
		}

		public FixedContentEditor Editor
		{
			get
			{
				return this.editor;
			}
		}

		public Block Block
		{
			get
			{
				return this.block;
			}
		}

		public double GetLineActualBaselineOffset()
		{
			switch (this.Block.LineSpacingType)
			{
			case HeightType.AtLeast:
				return this.Block.GetActualLineHeight(this.Line) - this.Line.Descent;
			case HeightType.Exact:
			{
				double num = this.Block.LineSpacing / (this.Line.BaselineOffset + this.Line.LineSpacingDescent);
				return this.Block.LineSpacing - this.Line.LineSpacingDescent * num;
			}
			default:
				return this.Line.BaselineOffset;
			}
		}

		readonly Block block;

		readonly FixedContentEditor editor;

		readonly LineInfo line;

		readonly double offsetX;

		readonly double offsetY;

		readonly Rect boundingRectangle;
	}
}
