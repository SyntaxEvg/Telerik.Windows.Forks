using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Graphics;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Flow
{
	class ParagraphInfo
	{
		public ParagraphInfo()
		{
			this.Clear();
		}

		public bool IsEmpty { get; set; }

		public bool HasBackground { get; set; }

		public ColorBase BackgroundColor { get; set; }

		public double SpacingBefore { get; set; }

		public double SpacingAfter { get; set; }

		public double LeftIndent { get; set; }

		public double RightIndent { get; set; }

		public double ActualWidth { get; set; }

		public void Copy(IBlockElement blockElement)
		{
			Block block = blockElement as Block;
			if (block == null)
			{
				this.Clear();
				return;
			}
			this.HasBackground = block.HasBackground;
			this.BackgroundColor = block.BackgroundColor;
			this.SpacingBefore = block.SpacingBefore;
			this.SpacingAfter = block.SpacingAfter;
			this.LeftIndent = block.LeftIndent;
			this.RightIndent = block.RightIndent;
			this.ActualWidth = block.ActualSize.Width;
			this.IsEmpty = false;
		}

		public Path CreateAdditionalBackgroundPath(double offsetX, double offsetY)
		{
			return Block.CreateBackgroundPath(new Rect(offsetX + this.LeftIndent, offsetY - this.SpacingAfter, this.ActualWidth - this.LeftIndent - this.RightIndent, this.SpacingAfter), this.BackgroundColor);
		}

		public void Clear()
		{
			this.IsEmpty = true;
		}
	}
}
