using System;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Flow
{
	class SpacingsCalculator
	{
		public SpacingsCalculator(SectionInfo section)
		{
			Guard.ThrowExceptionIfNull<SectionInfo>(section, "section");
			this.section = section;
			this.previousParagraph = new ParagraphInfo();
			this.Clear();
			this.isFirstBlock = false;
		}

		public void SetPreviousBlock(IBlockElement blockElement)
		{
			this.previousParagraph.Copy(blockElement);
			this.isFirstBlock = false;
		}

		public double GetSpacingBefore(Block block)
		{
			Guard.ThrowExceptionIfNull<Block>(block, "block");
			if (this.ShouldApplySpacingBefore(block))
			{
				double previousBlockSpacingAfter = this.GetPreviousBlockSpacingAfter();
				double spacingBefore = block.SpacingBefore;
				double num = Math.Max(previousBlockSpacingAfter, spacingBefore);
				return num - previousBlockSpacingAfter;
			}
			return 0.0;
		}

		bool ShouldApplySpacingBefore(Block block)
		{
			Guard.ThrowExceptionIfNull<Block>(block, "block");
			return !this.isFirstBlock || this.section.IsFirstPage();
		}

		double GetPreviousBlockSpacingAfter()
		{
			if (!this.previousParagraph.IsEmpty)
			{
				return this.previousParagraph.SpacingAfter;
			}
			return 0.0;
		}

		public bool ShouldExpandBackground(Block block)
		{
			return block.HasBackground && !this.previousParagraph.IsEmpty && this.previousParagraph.HasBackground && block.LeftIndent.IsEqualTo(this.previousParagraph.LeftIndent, 1E-08) && block.RightIndent.IsEqualTo(this.previousParagraph.RightIndent, 1E-08);
		}

		public Path GetExpandedBackground(double offsetX, double offsetY)
		{
			return this.previousParagraph.CreateAdditionalBackgroundPath(offsetX, offsetY);
		}

		public void Clear()
		{
			this.previousParagraph.Clear();
		}

		public void BeginExportContent()
		{
			this.Clear();
			this.isFirstBlock = true;
		}

		readonly SectionInfo section;

		readonly ParagraphInfo previousParagraph;

		bool isFirstBlock;
	}
}
